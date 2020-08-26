using System;

namespace GuildMaster.Characters
{
    [Serializable]
    public class CharacterBattleStatData
    {
        public int MaxHp;
        public bool SpIsMp;            // 참이면 Mp사용, 거짓이면 Dp 사용.
        public int MaxSp;
        public int MaxStamina;
        
        public int BaseAtk;
        public int BaseDef;
        public int BaseAgi;
        public int BaseInt;
    }
}