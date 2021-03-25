using System;
using System.Collections;
using System.Collections.Generic;
using GuildMaster.Characters;

namespace GuildMaster.Data
{
    public class MedicalBed
    {
        public event Action Changed;
        
        public MedicalBed(int capacity)
        {
            _onBeds = new Character[capacity];
        }

        public IReadOnlyList<Character> OnBeds => _onBeds;

        /// <summary>
        /// index번째 칸에 지정한 캐릭터를 넣고 원래 있던 애를 쫓아내고 그 애를 반환함.
        /// </summary>
        /// <param name="index"> 넣는 칸 </param>
        /// <param name="character"> 넣는 캐릭터 </param>
        /// <returns></returns>
        public Character PutCharacter(int index, Character character)
        {
            // 원래 있던 애 가져오기
            var prev = _onBeds[index];

            // 있는 Bed중 새로 넣는 캐릭터가 이미 있으면 쫓아냄(중복방지)
            if (character != null)
            {
                for (var i=0; i<_onBeds.Length; i++)
                    if (_onBeds[i] == character)
                        _onBeds[i] = null;
            }
            
            // 새로운 캐릭터 지정.
            _onBeds[index] = character;

            Changed?.Invoke();
            return prev;
        }

        /// <summary>
        /// index번째 칸에 지정된 캐릭터를 쫓아냄.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Character PopCharacter(int index)
        {
            return PutCharacter(index, null);
        }
        
        
        private readonly Character[] _onBeds;
    }
}