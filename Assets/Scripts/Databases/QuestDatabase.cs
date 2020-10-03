using System;
using GuildMaster.Databases;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "ScriptableObjects/QuestDatabase", order = 0)]
    public class QuestDatabase : UnityEditableIndexDatabase<QuestDatabase, QuestStaticData> {}

    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseEditor : DatabaseEditor {}
    
    [Serializable]
    public class QuestCode : QuestDatabase.Index {}

    [CustomPropertyDrawer(typeof(QuestDatabase.Index))]
    [CustomPropertyDrawer(typeof(QuestCode))]
    public class QuestCodeDrawer : DatabaseIndexDrawer<QuestDatabase, QuestStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, QuestStaticData element) =>
            $"Quest {i}: {element.QuestName}";
    }
}