using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Data;
using UnityEngine;
using Random = System.Random;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 탐색에서 일어나는 이벤트를 처리합니다.
    /// </summary>
    public class EventProcessor
    {
        public EventProcessor(ExplorationView explorationView, ReadOnlyCollection<Character> characters,
            Inventory inventory)
        {
            _explorationView = explorationView;
            _characters = characters;
            _inventory = inventory;
            _randomGenerator = new Random();
        }

        public async Task ProcessEvent(Event ev)
        {
            var choicesList = ev.Choices.Select(choice => new ExplorationView.ChoiceVisualData
            {
                Description = choice.Description,
                CharacterSelectHelperStrings =
                    _characters.Select(c => (c, InstructionToText(choice.Instruction, c))).ToList(),
            }).ToList();
            var (choiceIndex, selectedCharacter) =
                await _explorationView.PlayEvent(choicesList, ev.ShortDescription);
            Debug.Log(choiceIndex);
            Debug.Log(selectedCharacter.UsingName);
            await _explorationView.Notify("뭐가 어떻게 됐고 뭘 얻었고 어쩌고 저쩌고");
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
            string ExpressionToText(Expression expression)
                => ExpressionProcessor.Calculate(expression, character).ToString();

            switch (instruction)
            {
                case Instruction.PerChance perChance:
                    if (perChance.Failure == null)
                        return
                            $"{ExpressionToText(perChance.Chance)}% 확률로 ({InstructionToText(perChance.Success, character)})";
                    else
                        return
                            $"{ExpressionToText(perChance.Chance)}% 확률로 ({InstructionToText(perChance.Success, character)})" +
                            $", 실패시 ({InstructionToText(perChance.Success, character)})";
                case Instruction.ChangeEnergy changeEnergy:
                    var amount = ExpressionProcessor.Calculate(changeEnergy.Amount, character);
                    return $"{changeEnergy.TargetType} {Math.Abs(amount)} {(amount > 0 ? "증가" : "감소")}";
                case Instruction.GetItem getItem:
                    return $"아이템 [{getItem.Item.StaticData.ItemName}] {ExpressionToText(getItem.Number)}개 획득";
                case Instruction.EndEvent endEvent:
                    return "이벤트 종료.";
                case null:
                    return "null";
                default:
                    throw new Exception($"Couldn't follow {nameof(Instruction)} {instruction}");
            }
        }


        /// <summary>
        /// 지시를 수행합니다.
        /// </summary>
        /// <param name="instruction"> 지시 </param>
        /// <param name="selectedCharacter"> 선택한 캐릭터 </param>
        /// <returns> 이벤트를 끝내는지. </returns>
        /// <exception cref="Exception"> 처리 불가능한 지시. 제대로 구현되었다면 불리지 않음. </exception>
        private bool FollowInstruction(Instruction instruction, Character selectedCharacter)
        {
            int Calculate(Expression expression)
                => ExpressionProcessor.Calculate(expression, selectedCharacter);

            switch (instruction)
            {
                case null:
                    return false;
                case Instruction.PerChance perChance:
                    if (_randomGenerator.Next(0, 100) < Calculate(perChance.Chance))
                    {
                        return FollowInstruction(perChance.Success, selectedCharacter);
                    }
                    else
                    {
                        return FollowInstruction(perChance.Failure, selectedCharacter);
                    }
                case Instruction.ChangeEnergy changeEnergy:
                    // 왜 Property를 ref로 받을 수 없는가?
                    // 그럼 Property get set에 접근할 수 있는 클래스라도 있어야 하는 것 아닌가?
                    switch (changeEnergy.TargetType)
                    {
                        case Instruction.ChangeEnergy.EnergyType.Hp:
                            selectedCharacter.Hp += Calculate(changeEnergy.Amount);
                            break;
                        case Instruction.ChangeEnergy.EnergyType.Stamina:
                            selectedCharacter.Stamina -= Calculate(changeEnergy.Amount);
                            break;
                    }

                    return false;
                case Instruction.GetItem getItem:
                    _inventory.TryAddItem(getItem.Item, Calculate(getItem.Number));
                    return false;
                case Instruction.EndEvent endEvent:
                    return true;
                default:
                    throw new Exception($"Couldn't follow {nameof(Instruction)} {instruction}");
            }
        }

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


        private readonly ExplorationView _explorationView;
        private readonly ReadOnlyCollection<Character> _characters;
        private readonly Inventory _inventory;
        private readonly System.Random _randomGenerator;
    }
}