using GuildMaster.Data; //for test
using GuildMaster.InGameEvents; //for test
using GuildMaster.UI;
using UnityEditor; //for test
using UnityEngine;

namespace GuildMaster
{
    public class GameInput: MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                UiWindowsManager.Instance.questListWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.I))
                UiWindowsManager.Instance.inventoryWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.P))
                UiWindowsManager.Instance.characterInspectWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.G))
                UiWindowsManager.Instance.guildInspectWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.Escape))
                UiWindowsManager.Instance.settingWindow.Toggle();

            //For Test InGameEvents
            if (Input.GetKeyDown(KeyCode.T))
            {
                // Player.Instance.InGameEventManager.Occur(inGameEventData);
            }
        }
    }
}