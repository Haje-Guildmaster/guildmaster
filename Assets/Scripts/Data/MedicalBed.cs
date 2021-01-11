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
            var prev = _onBeds[index];
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