using System;
using GuildMaster.Npcs;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "NpcDatabase", menuName = "ScriptableObjects/NpcDatabase", order = 0)]
    public class NpcDatabase : UnityEditableIndexDatabase<NpcDatabase, NpcStaticData>
    {}


    [CustomEditor(typeof(NpcDatabase))]
    public class NpcDatabaseEditor : DatabaseEditor {}
    
    
    [Serializable]
    public class NpcCode : NpcDatabase.Index {}
    
    [CustomPropertyDrawer(typeof(NpcDatabase.Index))]
    [CustomPropertyDrawer(typeof(NpcCode))]
    public class NpcCodeDrawer : DatabaseIndexDrawer<NpcDatabase, NpcStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, NpcStaticData element) =>
            $"Npc {i}: {element.basicData.npcName}";
    }
}