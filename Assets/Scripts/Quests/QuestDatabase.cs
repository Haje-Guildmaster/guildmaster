using System;
using GuildMaster.Database;
using GuildMaster.Npcs;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Quests
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