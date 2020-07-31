using System;
using GuildMaster.InGameEvents;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = nameof(ExplorationLocationDatabase),
        menuName = "ScriptableObjects/" + nameof(ExplorationLocationDatabase), order = 0)]
    public class
        ExplorationLocationDatabase : UnityEditableDatabase<ExplorationLocationDatabase, Exploration.Location,
            LocationCode>
    {
    }

    [CustomEditor(typeof(ExplorationLocationDatabase))]
    public class ExplorationLocationDatabaseEditor : DatabaseEditor<ExplorationLocationDatabase, Exploration.Location,
        LocationCode>
    {
    }

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