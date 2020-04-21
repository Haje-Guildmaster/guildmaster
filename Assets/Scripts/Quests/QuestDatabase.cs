using System;
using GuildMaster.Database;
using GuildMaster.Npcs;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Quests
{
    [Serializable]
    public class QuestCode : QuestDatabase.Index {}
    
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "ScriptableObjects/QuestDatabase", order = 0)]
    public class QuestDatabase: UnityEditableDatabase<QuestDatabase, QuestStaticData, QuestCode> {}
    
    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseEditor : DatabaseEditor<QuestDatabase, QuestStaticData, QuestCode> {}
}