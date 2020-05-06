using System;
using System.Linq;
using GuildMaster.Characters;
using GuildMaster.Database;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Database
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "ScriptableObjects/CharacterDatabase", order = 0)]
    public class CharacterDatabase : UnityEditableDatabase<CharacterDatabase, CharacterStaticData, CharacterCode>
    {
    }

    [CustomEditor(typeof(CharacterDatabase))]
    public class CharacterDatabaseEditor : DatabaseEditor<CharacterDatabase, CharacterStaticData, CharacterCode>
    {
    }

    [Serializable]
    public class CharacterCode : CharacterDatabase.Index
    {
    }

    [CustomPropertyDrawer(typeof(CharacterCode))]
    public class CharacterCodeDrawer : DatabaseIndexDrawer<CharacterDatabase, CharacterStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, CharacterStaticData element)
            => $"Character {i}: {element.basicData.RealName ?? "null"}";
    }
}