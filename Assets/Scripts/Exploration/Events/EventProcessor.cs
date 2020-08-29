using System;
using System.Collections.ObjectModel;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Data;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 탐색에서 일어나는 이벤트를 처리합니다.
    /// </summary>
    /// <Note>
    /// 왜 코드가 이꼴이 났냐고 물어보신다면 저도 모르겠습니다. 뭔가 몇가지 조건을 만족시킬려고 했을 뿐인데 그걸 만족시킬려다
    /// 무언가 괴상한 코드가 되었습니다.
    /// </Note>
    public class EventProcessor
    {
        public EventProcessor(ExplorationView explorationView, ReadOnlyCollection<Character> characters, Inventory inventory)
        {
            _explorationView = explorationView;
            _characters = characters;
            _inventory = inventory;
            _randomGenerator = new Random();
        }

        public async void StartEvent()
        {
            
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
            switch (instruction)
            {
                case Instruction.PerChance perChance:
                    ApplyModifier(perChance);
                    if (_randomGenerator.NextDouble() > perChance.Chance)
                    {
                        return FollowInstruction(perChance.Success, selectedCharacter);
                    }
                    else
                    {
                        return FollowInstruction(perChance.Failure, selectedCharacter);
                    }
                case Instruction.ChangeEnergy changeEnergy:
                    ApplyModifier(changeEnergy);

                    // 왜 Property를 ref로 받을 수 없는가?
                    // 그럼 Property get set에 접근할 수 있는 클래스라도 있어야 하는 것 아닌가?
                    switch (changeEnergy.TargetType)
                    {
                        case Instruction.ChangeEnergy.EnergyType.Hp:
                            selectedCharacter.Hp -= changeEnergy.Amount;
                            break;
                        case Instruction.ChangeEnergy.EnergyType.Stamina:
                            selectedCharacter.Stamina -= changeEnergy.Amount;
                            break;
                    }

                    return false;
                case Instruction.GetItem getItem:
                    ApplyModifier(getItem);
                    _inventory.TryAddItem(getItem.Item, getItem.Number);
                    return false;
                case Instruction.EndEvent endEvent:
                    ApplyModifier(endEvent);
                    return true;
                default:
                    throw new Exception($"Couldn't follow {nameof(Instruction)} {instruction}");
            }
        }

        private void ApplyModifier(Instruction _)
        {
            /* Do Nothing */
        }

        private void ApplyModifier<T>(T instr) where T : Instruction.PreModifiableInstruction<T>
        {
            foreach (var modifier in instr.Modifiers.Where(modifier => CheckCondition(modifier.Condition)))
            {
                modifier.Modify(instr);
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


        private ExplorationView _explorationView;
        private ReadOnlyCollection<Character> _characters;
        private Inventory _inventory;

        private System.Random _randomGenerator;
    }
}