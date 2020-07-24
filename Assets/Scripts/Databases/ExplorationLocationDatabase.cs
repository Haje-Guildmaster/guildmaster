using System;
using GuildMaster.InGameEvents;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "nameof(ExplorationLocationDatabase)",
        menuName = "ScriptableObjects/ExplorationLocationDatabase", order = 0)]
    public class
        ExplorationLocationDatabase : UnityEditableDatabase<ExplorationLocationDatabase, Exploration.Location,
            LocationCode>
    {
    }

    [CustomEditor(typeof(Databases.InGameEventDatabase))]
    public class ExplorationLocationDatabaseEditor : DatabaseEditor<ExplorationLocationDatabase, Exploration.Location,
        LocationCode>
    {
    }

    // 유니티 serialization을 위해.
    [Serializable]
    public class LocationCode : ExplorationLocationDatabase.Index
    {
    }

    [CustomPropertyDrawer(typeof(LocationCode))]
    public class LocationCodeDrawer : DatabaseIndexDrawer<ExplorationLocationDatabase, Exploration.Location>
    {
        protected override string GetElementDescriptionWithIndex(int i, Exploration.Location element) =>
            $"Location {i}: [{element.LocationType}] {element.Name}";
    }
}