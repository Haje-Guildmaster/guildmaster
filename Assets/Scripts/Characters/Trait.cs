using System;
using UnityEngine;

namespace GuildMaster.Characters
{
    public enum Trait
    {
        Anger, Anger2, Unstop, Strong, Sleepy, Sleeping, Nothing, TrapDismantle
    }
    
    [Serializable]
    public class TraitStaticData
    {
        public string Name;
        [TextArea(2, 10)] public string Description;
    }
}