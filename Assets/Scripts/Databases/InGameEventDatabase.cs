using System;
using GuildMaster.InGameEvents;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "InGameEventDatabase", menuName = "ScriptableObjects/InGameEventDatabase", order = 0)]
    public class InGameEventDatabase : UnityEditableIndexDatabase<InGameEventDatabase, InGameEventStaticData>
    {}
    
    [CustomEditor(typeof(InGameEventDatabase))]
    public class InGameEventDatabaseEditor : DatabaseEditor
    {
    }

    // 유니티 serialization을 위해.
    [Serializable]
    public class EventCode : InGameEventDatabase.Index {}

    [CustomPropertyDrawer(typeof(InGameEventDatabase.Index))]
    [CustomPropertyDrawer(typeof(EventCode))]
    public class EventCodeDrawer : DatabaseIndexDrawer<InGameEventDatabase, InGameEventStaticData>
    {
        protected override string GetElementDescriptionWithIndex(int i, InGameEventStaticData element) =>
            $"Event {i}: {element.EventName}";
    }
}