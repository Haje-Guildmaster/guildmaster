using GuildMaster.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTraitData: MonoBehaviour
{
    // public CharacterTraitData Instance =>;
    
   
    public enum Trait
    {
        t1, t2, t3, t4, t5, t6,
    }

    [Serializable]
    public struct TRAITDATA
    {
        public string Name;
        [TextArea(2, 10)] public string Description;
    }
    [SerializeField]
    private List<TRAITDATA> NameMap = new List<TRAITDATA>();

    public string GetName(Trait trait) => NameMap[(int)trait].Name;
    public string GetDescription(Trait trait) => NameMap[(int)trait].Description;
}