using System;
using GuildMaster.Databases;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "ScriptableObjects/QuestDatabase", order = 0)]
    public class QuestDatabase : UnityEditableDatabase<QuestDatabase, QuestStaticData, QuestCode> {}

    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseEditor : DatabaseEditor<QuestDatabase, QuestStaticData, QuestCode> {}
    
    [Serializable]
    public class QuestCode : QuestDatabase.Index {}

    [CustomPropertyDrawer(typeof(QuestCode))]
    public class QuestCodeDrawer : DatabaseIndexDrawer<QuestDatabase, QuestStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, QuestStaticData element) =>
            $"Quest {i}: {element.QuestName}";
    }
}