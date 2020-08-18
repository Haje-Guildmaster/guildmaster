using System;
using GuildMaster.Data;
using GuildMaster.Windows;
using UnityEngine;

namespace GuildMaster.InGameEvents
{
    public class InGameEvent
    {
        public InGameEvent(InGameEventStaticData inGameEventData)
        {
            this.InGameEventData = inGameEventData;
            this.currentSceneNum = 0;
        }

        public readonly InGameEventStaticData InGameEventData;
        public int currentSceneNum;
        public InGameEventStaticData.InGameEventSceneData currentSceneData => InGameEventData.Scenes[currentSceneNum];

        public void Choose(int choice)
        {
            if (InGameEventData.Scenes.Count <= choice || choice < 0)
                throw new Exception("There is wrong choice");
            var currentChoice = currentSceneData.Choices[choice];
            if (currentChoice.NextStep is int ns)
            {
                currentSceneNum = ns;
            }
            else
            {
                Player.Instance.InGameEventManager.End();
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