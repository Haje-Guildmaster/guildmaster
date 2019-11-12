using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public abstract class CharacterBase : ObjectBase
    {
        public abstract void GetDamage(int damage);
    }
}