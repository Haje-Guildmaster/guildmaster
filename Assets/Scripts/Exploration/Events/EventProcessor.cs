using System;
using System.Linq;
using UnityEngine;
using Object = System.Object;

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
        private void FollowInstruction(Instruction instruction)
        {
            switch (instruction)
            {
                case Instruction.PerChance perChance:
                    ApplyModifier(perChance);
                    break;
                case Instruction.ChangeEnergy changeEnergy:
                    ApplyModifier(changeEnergy);

                    break;
                case Instruction.EndEvent endEvent:
                    ApplyModifier(endEvent);

                    break;
                case Instruction.GetItem getItem:
                    ApplyModifier(getItem);

                    break;
                default:
                    throw new Exception($"Couldn't follow {nameof(Instruction)} {instruction}");
            }
        }

        private void ApplyModifier(Instruction _) {/* Do Nothing */}
        private void ApplyModifier<T>(T instr) where T: Instruction.PreModifiableInstruction<T>
        {
            foreach (var modifier in instr.Modifiers.Where(modifier => CheckCondition(modifier.Condition)))
            {
                modifier.Modify(instr);
            }
        }
        
        private bool CheckCondition(Condition condition)
        {
            return true;
        }
    }
}