using System;
using GuildMaster.Npcs;
using UnityEditor;
using UnityEngine;
using GuildMaster.Dialogs;

namespace GuildMaster.Databases
{
    [CreateAssetMenu(fileName = "DialogDatabase", menuName = "ScriptableObjects/DialogDatabase", order = 0)]
    public class DialogDatabase : UnityEditableIndexDatabase<DialogDatabase, Dialog>
    { }


    [CustomEditor(typeof(DialogDatabase))]
    public class DialogDatabaseEditor : DatabaseEditor { }


    [Serializable]
    public class DialogCode : DialogDatabase.Index { }

    [CustomPropertyDrawer(typeof(DialogDatabase.Index))]
    [CustomPropertyDrawer(typeof(DialogCode))]
    public class DialogCodeDrawer : DatabaseIndexDrawer<DialogDatabase, Dialog>
    {
        protected override string GetElementDescriptionWithIndex(int i, Dialog element) =>
            $"대화 {i}:";
    }
}