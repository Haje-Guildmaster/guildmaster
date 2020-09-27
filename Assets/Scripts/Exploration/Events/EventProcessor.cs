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
    /// <summary>
    /// 탐색에서 일어나는 이벤트를 처리합니다.
    /// </summary>
    public class EventProcessor
    {
        public EventProcessor(ExplorationView explorationView, ReadOnlyCollection<Character> characters,
            Inventory inventory, Event ev, ExplorationLog log = null)
        {
            _explorationView = explorationView;
            _characters = characters;
            _inventory = inventory;
            _randomGenerator = new Random();
            _ev = ev;
            _log = log;
        }

        public async Task Run()
        {
            ExplorationView.ChoiceVisualData ChoiceToVisual(Event.Choice choice) =>
                new ExplorationView.ChoiceVisualData
                {
                    Description = choice.Description,
                    CharacterSelectHelperStrings =
                        _characters.Select(c => (c, InstructionToText(choice.Instruction, c))).ToList(),
                    // Todo: 일회용 여부 전달?
                };

            var choices = _ev.Choices.ToList(); // 선택지 복사. 변경가능하기에.

            var resultRecord = new ResultRecord();

            var lastChoice = 0;
            while (true)
            {
                var (choiceIndex, selectedCharacter) = await _explorationView.PlayEvent(choices.Select(ChoiceToVisual),
                    _ev.ShortDescription, lastChoice); // Select 시간복잡도 n 주의. 성능문제가 발생한다면 캐싱 고려.
                lastChoice = choiceIndex;

                Assert.IsTrue(choiceIndex < choices.Count);
                Assert.IsTrue(_characters.Contains(selectedCharacter));

                var choice = choices[choiceIndex];
                var (endEvent, flavorTexts, newChoices) =
                    FollowInstruction(choice.Instruction, selectedCharacter, resultRecord);
                await NotifyActionResult(flavorTexts);
                choices.InsertRange(choiceIndex+1, newChoices);
                if (choice.OneOff)
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
                    return "새로운 선택지 추가";
                case null:
                    return "null";
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
        private (bool FinishEvent, List<FlavorText> FlavorTexts, List<Event.Choice> newChoices) FollowInstruction(Instruction instruction,
            Character selectedCharacter, ResultRecord resultRecord)
        {
            var flavorTexts = new List<FlavorText>();
            var newChoices = new List<Event.Choice>();
            
            {    // 스코프 제한용.
                var finishEvent = FollowInstr(instruction);
                return (finishEvent, flavorTexts, newChoices);
            }
            
            bool FollowInstr(Instruction instr)
            {
                if (instr.FlavorText.Exist)
                    flavorTexts.Add(instr.FlavorText);

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
                    case Instruction.EndEvent endEvent:
                        return true;
                    case Instruction.DoNothing doNothing:
                        return false;
                    case Instruction.Sequential sequential:
                    {
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
                    default:
                        throw new Exception($"Couldn't follow {nameof(Instruction)} {instr}");
                }
            }
        }

        private float Calculate(Expression expression, Character selectedCharacter) =>
            ExpressionProcessor.Calculate(expression, selectedCharacter);

        private int CalculateToInt(Expression expression, Character selectedCharacter) =>
            (int) Math.Round(Calculate(expression, selectedCharacter), 0);

        private bool CheckCondition(Condition condition)
        {
            switch (condition)
            {
                case Condition.Always always:
                    return always.IsTrue;
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