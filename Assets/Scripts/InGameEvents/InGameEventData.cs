using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    [CreateAssetMenu(fileName = "InGameEvent", menuName = "ScriptableObjects/InGameEventData", order = 0)]
    public class InGameEventData : ScriptableObject
    {
        [SerializeField] private string eventName;
        [SerializeField] private InGameEventSceneData Firststep;
    }

}