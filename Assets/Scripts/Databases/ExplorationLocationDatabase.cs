using System;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = nameof(ExplorationLocationDatabase),
        menuName = "ScriptableObjects/" + nameof(ExplorationLocationDatabase), order = 0)]
    public class
        ExplorationLocationDatabase : UnityEditableIndexDatabase<ExplorationLocationDatabase, Exploration.Location>
    {
    }

    [CustomEditor(typeof(ExplorationLocationDatabase))]
    public class ExplorationLocationDatabaseEditor : DatabaseEditor
    {
    }

    [Serializable]
    public class LocationCode : ExplorationLocationDatabase.Index
    {
    }

    [CustomPropertyDrawer(typeof(ExplorationLocationDatabase.Index))]
    [CustomPropertyDrawer(typeof(LocationCode))]
    public class LocationCodeDrawer : DatabaseIndexDrawer<ExplorationLocationDatabase, Exploration.Location>
    {
        protected override string GetElementDescriptionWithIndex(int i, Exploration.Location element) =>
            $"Location {i}: [{element.LocationType}] {element.Name}";
    }
}