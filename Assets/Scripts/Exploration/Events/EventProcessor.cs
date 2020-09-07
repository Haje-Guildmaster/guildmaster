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
            var testChoicesList = new List<ExplorationView.ChoiceVisualData>
            {
                new ExplorationView.ChoiceVisualData
                {
                    Description = "무시하고 지나간다",
                    CharacterSelectHelperStrings = _characters.Select(c=>(c, "이벤트 종료")).ToList(),
                }
            };
            var (choiceIndex, selectedCharacter) = await _explorationView.PlayEvent(testChoicesList);
            Debug.Log(choiceIndex);
            Debug.Log(selectedCharacter.UsingName);
            await _explorationView.Notify("뭐가 어떻게 됐고 뭘 얻었고 어쩌고 저쩌고");
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
                    // ApplyModifier(perChance);
                    if (_randomGenerator.Next(0, 100) < Calculate(perChance.Chance))
                    {
                        return FollowInstruction(perChance.Success, selectedCharacter);
                    }
                    else
                    {
                        return FollowInstruction(perChance.Failure, selectedCharacter);
                    }
                case Instruction.ChangeEnergy changeEnergy:
                    // ApplyModifier(changeEnergy);

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
                    // ApplyModifier(getItem);
                    _inventory.TryAddItem(getItem.Item, Calculate(getItem.Number));
                    return false;
                case Instruction.EndEvent endEvent:
                    // ApplyModifier(endEvent);
                    return true;
                default:
                    throw new Exception($"Couldn't follow {nameof(Instruction)} {instruction}");
            }
        }

        // private void ApplyModifier(Instruction _)
        // {
        //     /* Do Nothing */
        // }

        // private void ApplyModifier<T>(T instr) where T : Instruction.PreModifiableInstruction<T>
        // {
        //     foreach (var modifier in instr.Modifiers.Where(modifier => CheckCondition(modifier.Condition)))
        //     {
        //         modifier.Modify(instr);
        //     }
        // }

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