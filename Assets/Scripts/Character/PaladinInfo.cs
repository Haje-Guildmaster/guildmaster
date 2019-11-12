using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public class PaladinInfo : CharacterInfoBase
    {
        public override int HP { get; set; }
        public override int ATK { get; set; }
        public override int DEF { get; set; }
        public override int SPD { get; set; }

        public override void Init()
        {
            HP = 10;
            ATK = 3;
            DEF = 2;
            SPD = 2;
        }
    }
}