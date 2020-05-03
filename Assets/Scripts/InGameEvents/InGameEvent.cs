using System;
using GuildMaster.Data;
using GuildMaster.UI;
using UnityEngine;

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
        public InGameEventSceneData currentSceneData => InGameEventData.Scenes[currentSceneNum];

        public void Choose(int choice)
        {
            if (InGameEventData.Scenes.Count <= choice || choice < 0)
                throw new Exception("There is wrong choice");
            InGameEventChoiceData currentChoice = currentSceneData.Choices[choice];
            if (currentChoice.HasNextStep)
            {
                currentSceneNum = currentChoice.NextStep;
            }
            else
            {
                PlayerData.Instance.InGameEventManager.End();
                return;
            }
            UiWindowsManager.Instance.inGameEventWindow.Refresh();
        }

        public void End()
        {
            Debug.Log("event end call");
        }
    }
}