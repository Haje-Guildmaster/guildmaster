using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public class Paladin : CharacterBase
    {
        public override ObjectInfoBase Info { get; set; }

        public Paladin()
        {
            this.Info = new PaladinInfo();
        }

        public override void GetDamage(int damage)
        {
            this.Info.HP = this.Info.HP - (damage - this.Info.DEF);
        }
    }
}
