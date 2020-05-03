﻿using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.InGameEvents
{
    [CreateAssetMenu(fileName = "InGameEvent", menuName = "ScriptableObjects/InGameEventData", order = 0)]
    public class InGameEventData : ScriptableObject
    {
        [SerializeField] private string eventName;
        [SerializeField] private List<InGameEventSceneData> scenes;

        public string EventName => eventName;
        public List<InGameEventSceneData> Scenes => scenes;
    }
}