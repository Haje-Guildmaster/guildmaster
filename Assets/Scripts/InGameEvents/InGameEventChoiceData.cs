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
        [SerializeField] private bool hasNextStep;
        [SerializeField] private int nextStep;

        public Script ChoiceScript => choiceScript;
        public bool HasNextStep => hasNextStep;
        public int NextStep
        {
            get
            {
                if (hasNextStep)
                    return -1;
                return nextStep;
            }
        }
    }
}