using System;
using UnityEngine;

namespace GuildMaster.TownRoam.TownModifiers
{
    /*
     * 맵을 조건에 따라 변형함.
     */
    public abstract class TownModifier
    {
        public abstract void Modify(Town town);
    }
}