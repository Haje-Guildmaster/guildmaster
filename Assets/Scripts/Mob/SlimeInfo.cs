using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public sealed class SlimeInfo : MobInfoBase
    {
        public override int HP { get; set; }
        public override int ATK { get; set; }
        public override int DEF { get; set; }
        public override int SPD { get; set; }

        public override void Init()
        {
            HP = 5;
            ATK = 3;
            DEF = 1;
            SPD = 4;
        }
    }
}