using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public class Slime : MobBase
    {
        public override ObjectInfoBase Info { get; set; }

        public Slime()
        {
            this.Info = new SlimeInfo();
        }

        public override void GetDamage(int damage)
        {
            this.Info.HP = this.Info.HP - (damage - this.Info.DEF);
        }
    }
}