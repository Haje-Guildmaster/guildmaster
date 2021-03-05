using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuildMaster.Characters;

namespace GuildMaster.Exploration.Events
{
    public static class ForcedExpEndTool
    {
        /// <summary>
        /// 탐색에 참여한 모든 캐릭터들을 대상으로 한 노드만큼 움직였을 때의 스태미나 감소 적용
        /// </summary>
        /// <param name="expParty"> 탐색에 참여한 캐릭터들을 담은 리스트 </param>
        /// <returns>  </returns>
        public static void DecreaseStaminaPerNode(IEnumerable<Character> expParty)
        {
            foreach (Character member in expParty)
            {
                member.Stamina -= 10;
            }
            return;
        }
        /// <summary>
        /// 탐색에 참여한 모든 캐릭터들을 대상으로 탐색을 지속할 수 있는 상황인지 점검
        /// '모든' 캐릭터가 탐색 진행 불가 조건(stamina 0 or hp 0)를 만족하는 경우 true를 반환.
        /// </summary>
        /// <param name="expParty"> 탐색에 참여한 캐릭터들을 담은 리스트 </param>
        /// <returns> 탐색을 지속할 수 있는 경우 false, 지속할 수 없는 경우 true 반환 </returns>
        public static bool CheckExpEndCond(IEnumerable<Character> expParty)
        {
            bool expEnd = true;
            foreach (Character member in expParty)
            {
                expEnd = expEnd && CheckCharacterExhausted(member);
            }
            return expEnd;

        }
        /// <summary>
        /// 한 캐릭터를 대상으로 탐색 중 발생하는 이벤트에 참여할 수 있는지 점검
        /// 캐릭터가 탐색 진행 불가 조건(stamina 0 or hp 0)를 만족하는 경우 true를 반환
        /// </summary>
        /// <param name="member"> 탐색에 참여한 캐릭터 하나 </param>
        /// <returns> 이벤트에 참여할 수 있는 경우 false, 참여할 수 없는 경우 true 반환 </returns>
        public static bool CheckCharacterExhausted(Character member)
        {
            return member.Stamina <= 0 || member.Hp <= 0;
        }
    }

}