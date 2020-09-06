using GuildMaster.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTraitData: MonoBehaviour
{
    // public CharacterTraitData Instance =>;
    
   
    public enum Trait
    {
        Anger, Anger2, Unstop, Strong, Sleepy, Sleeping, Nothing,
    }

    [Serializable]
    public struct TRAITDATA
    {
        public string Name;
        [TextArea(2, 10)] public string Description;
    }
    //public string GetName(Trait trait) => NameMap[(int)trait].Name;
    //public string GetDescription(Trait trait) => NameMap[(int)trait].Description;
}