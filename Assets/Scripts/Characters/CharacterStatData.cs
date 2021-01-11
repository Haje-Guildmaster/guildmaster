using System;

namespace GuildMaster.Characters
{

    ///<summary>
    ///캐릭터의 스텟을 저장하는 클래스입니다. 특성이나 장비에 의한 가감을 고려하지 않은 수치를 저장합니다.
    /// </summary>
    [Serializable]
    public class CharacterStatData
    {
        public int MaxHp;
        public int MaxStamina;
        
        public int Strength;
        public int Trick;
        public int Wisdom;

        public int Level;
        public int CurrentXP;
        public int[] LevelupXP;
        public int Loyalty;
    }
}