using System;
using GuildMaster.Dialog;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    [Serializable]
    public struct InGameEventChoiceData
    {
        [SerializeField] private Script choiceScript;
        public bool hasNextStep => false;
        [SerializeField] private InGameEventSceneData nextstep;
        
    }
}