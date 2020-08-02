

using System;

namespace GuildMaster.Characters
{
    [Serializable]
    public class CharacterBattleStatData
    {
        public int maxHp;
        public bool spIsMp;            // 참이면 Mp사용, 거짓이면 Dp 사용.
        public int maxSp;
        
        public int baseAtk;
        public int baseDef;
        public int baseAgi;
        public int baseInt;
    }
}