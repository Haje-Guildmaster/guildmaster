using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.InGameEvents
{
    [Serializable]
    public class InGameEventStaticData
    {
        public string EventName;
        public List<InGameEventSceneData> Scenes;

        [Serializable]
        public class InGameEventSceneData
        {
            public Sprite SceneSprite;
            public string SceneDescription;
            public List<InGameEventChoiceData> Choices;
            
            [Serializable]
            public struct InGameEventChoiceData
            {
                public string ChoiceScript;
                public int? NextStep;
            }
        }
    }
}