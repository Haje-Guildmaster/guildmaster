﻿using GuildMaster.Data;
using GuildMaster.UI;

namespace GuildMaster.InGameEvents
{
    public class InGameEventManager
    {
        public InGameEventManager(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        private readonly PlayerData playerData;
        public InGameEvent currentInGameEvent;

        public void Occur(InGameEventData inGameEventData)
        {
            if (this.currentInGameEvent != null)
                throw new System.Exception("now doing another event");
            currentInGameEvent = new InGameEvent(inGameEventData);
            UiWindowsManager.Instance.inGameEventWindow.Open();
        }

        public void Choose(int choice)
        {
            if (this.currentInGameEvent == null)
                throw new System.Exception("there is no current event");
            currentInGameEvent.Choose(choice);
        }

        public void End()
        {
            if (this.currentInGameEvent == null)
                throw new System.Exception("there is no current event");
            currentInGameEvent.End();
            UiWindowsManager.Instance.inGameEventWindow.Close();
            currentInGameEvent = null;
        }
    }
}