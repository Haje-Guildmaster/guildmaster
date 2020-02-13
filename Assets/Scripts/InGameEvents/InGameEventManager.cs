using GuildMaster.Data;
using GuildMaster.Events;
using GuildMaster.UI;
using UnityEngine;
using UnityEditor;

namespace GuildMaster.InGameEvents
{
    public class InGameEventManager
    {
        public InGameEventManager(PlayerData playerData)
        {
            this.playerData = playerData;
            GameEvents.InGameEventManagerEventOccur.AddListener(Occur);
        }

        private readonly PlayerData playerData;
        private InGameEvent currentInGameEvent;

        public void Occur() //InGameEventData inGameEventData
        {
            if (this.currentInGameEvent != null)
                throw new System.Exception("now doing another event");
            InGameEventData inGameEventData = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/ScriptableObjects/InGameEvent/CursedOldBook.asset") as InGameEventData;
            InGameEvent currentInGameEvent = new InGameEvent(inGameEventData);
            //UiWindowsManager.Instance.
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
            currentInGameEvent = null;
        }
    }
}