using System;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.TownRoam.TownModifiers
{
    /*
     * 객체를 변형함.
     */
    public abstract class Modifier<T>
    {
        public abstract void Modify(T obj);
    }
}