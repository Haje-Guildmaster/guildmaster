using System;
using System.Collections.Generic;
using GuildMaster.Dialog;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    [Serializable]
    public struct InGameEventSceneData
    {
        [SerializeField] private string inGameEventDescription;
        [SerializeField] private List<InGameEventChoiceData> choices;

        public string InGameEventDescription => inGameEventDescription;
        public List<InGameEventChoiceData> Choices => choices;
    }
}