using System;
using GuildMaster.Dialog;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    [Serializable]
    public struct InGameEventChoiceData
    {
        [SerializeField] private string choiceScript;
        [SerializeField] private Vector3 buttonPosition;
        [SerializeField] private bool hasNextStep;
        [SerializeField] private int nextStep;

        public string ChoiceScript => choiceScript;
        public Vector3 ButtonPosition => buttonPosition;
        public bool HasNextStep => hasNextStep;
        public int NextStep
        {
            get
            {
                if (!hasNextStep)
                    return -1;
                return nextStep;
            }
        }
    }
}