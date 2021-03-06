using GuildMaster.Characters;
using UnityEditor;
using UnityEngine;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "TraitDatabase", menuName = "ScriptableObjects/TraitDatabase", order = 0)]
    public class TraitDatabase: UnityEditableEnumDatabase<TraitDatabase, TraitStaticData, Trait>
    {}

    [CustomEditor(typeof(TraitDatabase))]
    public class TraitDatabaseEditor : DatabaseEditor
    {
    }
}