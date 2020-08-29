using System;
using GuildMaster.Exploration.Events;
using GuildMaster.InGameEvents;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [Serializable]
    public class EventSeedReference
    {
        public string Name;
        [SerializeReference] [SerializeReferenceButton]
        public EventSeed EventSeed;
    }

    [CreateAssetMenu(fileName = "EventSeedDatabase", menuName = "ScriptableObjects/EventSeedDatabase", order = 0)]
    public class EventSeedDatabase : UnityEditableDatabase<EventSeedDatabase, EventSeedReference, EventSeedCode>
    {
    }
    
    [CustomEditor(typeof(EventSeedDatabase))]
    public class EventSeedDatabaseEditor : DatabaseEditor<EventSeedDatabase, EventSeedReference, EventSeedCode>
    {
    }

    // 유니티 serialization을 위해.
    [Serializable]
    public class EventSeedCode : EventSeedDatabase.Index {}

    [CustomPropertyDrawer(typeof(EventSeedCode))]
    public class EventSeedCodeDrawer : DatabaseIndexDrawer<EventSeedDatabase, EventSeedReference>
    {
        protected override string GetElementDescriptionWithIndex(int i, EventSeedReference element) =>
            $"EventSeed {i}: {element.Name}";
    }
}