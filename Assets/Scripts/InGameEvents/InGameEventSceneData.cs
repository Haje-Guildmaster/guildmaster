using System;
using System.Collections.Generic;
using GuildMaster.Dialog;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    [CreateAssetMenu(fileName = "InGameEventScene", menuName = "ScriptableObjects/InGameEventSceneData", order = 0)]
    public class InGameEventSceneData : ScriptableObject
    {
        [SerializeField] private Script InGameEventDescription;
        [SerializeField] private List<InGameEventChoiceData> choices;
    }
}