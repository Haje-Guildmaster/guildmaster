using System;
using System.Collections.Generic;

namespace GuildMaster.Characters
{

    ///<summary>
    ///캐릭터의 스텟을 저장하는 클래스입니다. 특성이나 장비에 의한 가감을 고려하지 않은 수치를 저장합니다.
    /// </summary>
    [Serializable]
    public class CharacterStatData
    {
        public List<int> MaxHp;
        public List<int> MaxStamina;
        
        public List<int> Strength;
        public List<int> Trick;
        public List<int> Wisdom; 
        
        public int MaxLoyalty; //내부적인 최대 값. 보여지는 최대 값은 / 10
        public int MaxLevel;

        public List<int> LevelupXP;
    }
}