using System;
using GuildMaster.Characters;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "ScriptableObjects/CharacterDatabase", order = 0)]
    public class CharacterDatabase : UnityEditableIndexDatabase<CharacterDatabase, CharacterStaticData>
    {
    }

    [CustomEditor(typeof(CharacterDatabase))]
    public class CharacterDatabaseEditor : DatabaseEditor
    {
    }

    [Serializable]
    public class CharacterCode : CharacterDatabase.Index
    {
    }

    
    [CustomPropertyDrawer(typeof(CharacterDatabase.Index))]
    [CustomPropertyDrawer(typeof(CharacterCode))]
    public class CharacterCodeDrawer : DatabaseIndexDrawer<CharacterDatabase, CharacterStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, CharacterStaticData element)
            => $"Character {i}: {element.BasicData.RealName ?? "null"}";
    }
}