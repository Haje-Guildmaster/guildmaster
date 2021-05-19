using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Items;
using UnityEngine.Assertions;
using Random = System.Random;

namespace GuildMaster.Exploration.Events
{
    using Weighteditem = ProbabilityTool<Instruction>.Weighteditem;
    /// <summary>
    /// EventManager로부터 탐색 진행을 넘겨받아 탐색 이벤트 전체를 처리하고 다시 EventManager로 반환하는 역할입니다.
    /// </summary>
    public class EventProcessor
    {
        public EventProcessor(ExplorationView explorationView, ReadOnlyCollection<Character> characters,
            Inventory inventory, Event ev, Random random, ExplorationLog log = null)
        {
            _explorationView = explorationView;
            _characters = characters;
            _inventory = inventory;
            _randomGenerator = random;
            _ev = ev;
            _log = log;
        }

        /// <summary>
        /// 탐색을 진행하고 탐색이 끝나면 반환합니다.
        /// </summary>
        public async Task Run()
        {
            ExplorationView.ChoiceVisualData ChoiceToVisual(Event.Choice choice) =>
                new ExplorationView.ChoiceVisualData
                {
                    Description = choice.Description,
                    CharacterSelectHelperStrings =
                        _characters.Select(c => (c, CheckCondition(choice.ActivationCondition, c),
                            InstructionToText(choice.Sequential, c))).ToList(),
                };

            var choices = _ev.Choices.ToList(); // 선택지 복사. 변경가능하기에.

            var resultRecord = new ResultRecord();

            var lastChoice = 0;
            while (true)
            {
                var (choiceIndex, selectedCharacter) = await _explorationView.PlayEvent(choices.Select(ChoiceToVisual),
                    _ev.ShortDescription, lastChoice); // Select 시간복잡도 (선택지수)*(캐릭터수) 주의. 성능문제가 발생한다면 캐싱 고려.
                lastChoice = choiceIndex;

                Assert.IsTrue(choiceIndex < choices.Count);

                var choice = choices[choiceIndex];

                Assert.IsTrue(_characters.Contains(selectedCharacter));
                Assert.IsTrue(CheckCondition(choice.ActivationCondition, selectedCharacter));

                var (endEvent, flavorTexts, newChoices, removeCurrentChoice) =
                    FollowInstruction(choice.Sequential, selectedCharacter, resultRecord);
                await NotifyActionResult(flavorTexts);
                choices.InsertRange(choiceIndex + 1, newChoices);
                if (removeCurrentChoice)
                {
                    choices.RemoveAt(choiceIndex);
                    lastChoice = Math.Min(choiceIndex, choices.Count); // Max(0, choiceIndex-1)?
                }

                if (endEvent) break;
            }

            if (_log != null)
                resultRecord.WriteToLog(_log);

            await NotifyOverallResult(resultRecord);
        }

        
        /// <summary>
        /// 탐색 하나에서 일어난 일에 관한 기록.
        /// </summary>
        private class ResultRecord
        {
            public Dictionary<Item, int> AcquiredItems = new Dictionary<Item, int>();

            public void WriteToLog(ExplorationLog log)
            {
                void AddCountDictTo<T>(IDictionary<T, int> toDict, IDictionary<T, int> fromDict)
                {
                    foreach (var pair in fromDict)
                    {
                        toDict.TryGetValue(pair.Key, out var original);
                        toDict[pair.Key] = original + pair.Value;
                    }
                }

                AddCountDictTo(log.AcquiredItems, this.AcquiredItems);
            }
        }
        
        /// <summary>
        /// 탐색이 끝날 때, 탐색에서 바뀐 것들을 유저에게 보여줍니다.
        /// </summary>
        private async Task NotifyOverallResult(ResultRecord resultRecord)
        {
            var resultStr = "";
            foreach (var kvp in resultRecord.AcquiredItems)
            {
                var item = kvp.Key;
                var number = kvp.Value;

                resultStr += $"{item.StaticData.ItemName} {Math.Abs(number)}개 {(number > 0 ? "획득" : "상실")}\n";
            }

            if (resultStr == "")
                resultStr = "획득한 아이템이 없습니다.";

            resultStr = "결산\n\n" + resultStr;
            await _explorationView.Notify(resultStr);
        }

        private async Task NotifyActionResult(IEnumerable<FlavorText> flavorTexts)
        {
            var resultStr = "";
            foreach (var flavorText in flavorTexts)
            {
                resultStr += $"- {flavorText.Text}\n";
            }

            await _explorationView.Notify(resultStr);
        }

        /// <summary>
        /// 지시를 유저에게 보여줄 텍스트로 바꿉니다. (아마 나중엔 반환타입이 단순 텍스트가 아니게 되리라 생각합니다만)
        /// </summary>
        /// <param name="instruction"> 지시 </param>
        /// <param name="character"> 대상 캐릭터 </param>
        /// <returns> 이 캐릭터로 지시를 실행하였을 시 일어나는 일에 대한 비주얼 정보. </returns>
        /// <exception cref="Exception"> 처리 불가능한 지시. 제대로 구현되었다면 불리지 않음. </exception>
        private string InstructionToText(Instruction instruction, Character character)
        {
            switch (instruction)
            {
                case null:
                    return "";
                case Instruction.PerChance perChance:
                    if (perChance.Failure == null)
                        return
                            $"{Math.Floor(Calculate(perChance.Chance, character) * 100)}% 확률로 ({InstructionToText(perChance.Success, character)})";
                    else
                        return
                            $"{Math.Floor(Calculate(perChance.Chance, character) * 100)}% 확률로 ({InstructionToText(perChance.Success, character)})" +
                            $", 실패시 ({InstructionToText(perChance.Failure, character)})";
                case Instruction.ChangeEnergy changeEnergy:
                    var amount = Calculate(changeEnergy.Amount, character);
                    return $"{changeEnergy.TargetType} {Math.Abs((int) amount)} {(amount > 0 ? "증가" : "감소")}";
                case Instruction.GetItem getItem:
                    return $"아이템 [{getItem.Item.StaticData.ItemName}] {Calculate(getItem.Number, character)}개 획득";
                case Instruction.EndEvent endEvent:
                    return "이벤트 종료.";
                case Instruction.Sequential sequential:
                {
                    var instrs = sequential.Instructions;
                    switch (instrs.Count)
                    {
                        case 0:
                            return "";
                        case 1:
                            return InstructionToText(instrs[0], character);
                    }

                    var ret = "";
                    foreach (var inst in instrs)
                    {
                        var text = InstructionToText(inst, character);
                        if (text != "")
                            ret += $"- {text}\n";
                    }

                    return ret;
                }
                case Instruction.DoNothing doNothing:
                    return "";
                case Instruction.AddChoice addChoice:
                    return $"새로운 선택지 [{addChoice.Choice.Description}]";
                case Instruction.If ifInstr:
                    var matchCondition = CheckCondition(ifInstr.Condition, character);
                    return InstructionToText(matchCondition ? ifInstr.IfTrue : ifInstr.Else, character);
                case Instruction.RemoveCurrentChoice _:
                    return "현재 선택지 삭제";
                default:
                    throw new Exception($"Couldn't follow {nameof(Instruction)} {instruction}");
            }
        }


        /// <summary>
        /// 지시를 수행하고 resultRecord에 변경된 내용을 기록합니다.
        /// currentChoices를 변형할 수 있습니다.
        /// </summary>
        /// <param name="instruction"> 지시 </param>
        /// <param name="selectedCharacter"> 선택한 캐릭터 </param>
        /// <param name="resultRecord"> 결과 기록장 </param>
        /// <returns> 이벤트를 끝내는지. </returns>
        /// <exception cref="Exception"> 처리 불가능한 지시. 제대로 구현되었다면 불리지 않음. </exception>
        private (bool FinishEvent, List<FlavorText> FlavorTexts, List<Event.Choice> newChoices, bool removeCurrentChoice
            ) FollowInstruction(Instruction instruction, Character selectedCharacter, ResultRecord resultRecord)
        {
            var flavorTexts = new List<FlavorText>();
            var newChoices = new List<Event.Choice>();
            var removeCurrentChoice = false;

            {
                // 스코프 제한용.
                var finishEvent = FollowInstr(instruction);
                return (finishEvent, flavorTexts, newChoices, removeCurrentChoice);
            }

            bool FollowInstr(Instruction instr)
            {
                switch (instr)
                {
                    case null:
                        return false;
                    case Instruction.PerChance perChance:
                    {
                        if (_randomGenerator.NextDouble() < Calculate(perChance.Chance, selectedCharacter))
                        {
                            return FollowInstr(perChance.Success);
                        }
                        else
                        {
                            return FollowInstr(perChance.Failure);
                        }
                    }
                    case Instruction.Battle battle:
                    {
                            List<Weighteditem> instructions = new List<Weighteditem>();
                            
                            instructions.Add(new Weighteditem(battle.Success, (int)Calculate(battle.SuccessChance, selectedCharacter)));
                            instructions.Add(new Weighteditem(battle.BigSuccess, (int)Calculate(battle.BigSuccessChance, selectedCharacter)));
                            instructions.Add(new Weighteditem(battle.Failure, (int)Calculate(battle.FailureChance, selectedCharacter)));
                            instructions.Add(new Weighteditem(battle.BigFailure, (int)Calculate(battle.BigFailureChance, selectedCharacter)));

                            ProbabilityTool<Instruction> probabilityTool = new ProbabilityTool<Instruction>(instructions, _randomGenerator);
                            return FollowInstr(probabilityTool.Getitem().item);

                    }
                    case Instruction.ChangeEnergy changeEnergy:
                    {
                        // 왜 Property를 ref로 받을 수 없는가?
                        // 그럼 Property get set에 접근할 수 있는 클래스라도 있어야 하는 것 아닌가?
                        var amount = CalculateToInt(changeEnergy.Amount, selectedCharacter);
                        switch (changeEnergy.TargetType)
                        {
                            case Instruction.ChangeEnergy.EnergyType.Hp:
                                selectedCharacter.Hp += amount;
                                break;
                            case Instruction.ChangeEnergy.EnergyType.Stamina:
                                selectedCharacter.Stamina -= amount;
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        var key = (selectedCharacter, changeEnergy.TargetType);
                        return false;
                    }
                    case Instruction.GetItem getItem:
                    {
                        var number = CalculateToInt(getItem.Number, selectedCharacter);
                        if (_inventory.TryAddItem(getItem.Item, number))
                        {
                            var key = getItem.Item;
                            resultRecord.AcquiredItems.TryGetValue(key, out var original);
                            resultRecord.AcquiredItems[key] = original + number;
                        }

                        return false;
                    }
                    case Instruction.EndEvent _:
                        return true;
                    case Instruction.DoNothing _:
                        return false;
                    case Instruction.Sequential sequential:
                    {
                        if (sequential.FlavorText.Exist)
                            flavorTexts.Add(sequential.FlavorText);

                        var endEvent = false;
                        foreach (var subInstr in sequential.Instructions)
                        {
                            endEvent = FollowInstr(subInstr);
                            if (endEvent) break;
                        }

                        return endEvent;
                    }
                    case Instruction.AddChoice addChoice:
                    {
                        newChoices.Add(addChoice.Choice);
                        return false;
                    }
                    case Instruction.If ifInstr:
                    {
                        var matchCondition = CheckCondition(ifInstr.Condition, selectedCharacter);
                        return FollowInstr(matchCondition ? ifInstr.IfTrue : ifInstr.Else);
                    }
                    case Instruction.RemoveCurrentChoice _:
                    {
                        removeCurrentChoice = true;
                        return false;
                    }
                    default:
                        throw new Exception($"Couldn't follow {nameof(Instruction)} {instr}");
                }
            }
        }

        private float Calculate(Expression expression, Character selectedCharacter) =>
            ExpressionProcessor.Calculate(expression, selectedCharacter);

        private int CalculateToInt(Expression expression, Character selectedCharacter) =>
            (int) Math.Round(Calculate(expression, selectedCharacter), 0);

        private bool CheckCondition(Condition condition, Character selectedCharacter)
        {
            switch (condition)
            {
                case null:
                    return true;
                case Condition.Always always:
                    return always.IsTrue;
                case Condition.HasTrait hasTrait:
                    return selectedCharacter.ActiveTraits.Contains(hasTrait.Trait);
                default:
                    throw new Exception($"{nameof(CheckCondition)}: Couldn't check condition {condition}");
            }
        }

        private readonly ExplorationLog _log;
        private readonly Event _ev;
        private readonly ExplorationView _explorationView;
        private readonly ReadOnlyCollection<Character> _characters;
        private readonly Inventory _inventory;
        private readonly Random _randomGenerator;
    }
}