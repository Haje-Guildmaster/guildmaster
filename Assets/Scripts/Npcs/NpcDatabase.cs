using System;
using GuildMaster.Database;
using GuildMaster.Items;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [CreateAssetMenu(fileName = "NpcDatabase", menuName = "ScriptableObjects/NpcDatabase", order = 0)]
    public class NpcDatabase : UnityEditableDatabase<NpcDatabase, NpcStaticData, NpcCode>
    {}

    [Serializable]
    public class NpcCode : NpcDatabase.Index {}

    [CustomEditor(typeof(NpcDatabase))]
    public class NpcDatabaseEditor : DatabaseEditor<NpcDatabase, NpcStaticData, NpcCode> {}
}