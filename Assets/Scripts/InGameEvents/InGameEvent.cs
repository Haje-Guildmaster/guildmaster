using System;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    public class InGameEvent
    {
        public InGameEvent(InGameEventData inGameEventData)
        {
            this.InGameEventData = inGameEventData;
            this.currentSceneNum = 0;
        }

        public readonly InGameEventData InGameEventData;
        public int currentSceneNum;

        public void Choose(int choice)
        {
            if (InGameEventData.Scenes.Count < choice || choice < 0)
                throw new Exception("There is wrong choice");
            InGameEventSceneData currentScene = InGameEventData.Scenes[currentSceneNum];
            InGameEventChoiceData currentChoice = currentScene.Choices[choice];
            if (currentChoice.HasNextStep)
            {
                currentSceneNum = currentChoice.NextStep;
            }
            else
            {
                End();
            }
        }

        public void End()
        {
            
        }
    }
}