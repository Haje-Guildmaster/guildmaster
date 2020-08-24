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
                UiWindowsManager.Instance .guildInspectWindow.Toggle();
            //Settings 와 그 자식 파일들 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (UiWindowsManager.Instance.TextureWindow.IsOpen)
                {
                    UiWindowsManager.Instance.TextureWindow.Toggle();
                }
                else if (UiWindowsManager.Instance.MvWindow.IsOpen)
                {
                    UiWindowsManager.Instance.MvWindow.Toggle();
                }
                else if (UiWindowsManager.Instance.ResolutionWindow.IsOpen)
                {
                    UiWindowsManager.Instance.ResolutionWindow.Toggle();
                }
                else if (UiWindowsManager.Instance.SeWindow.IsOpen)
                {
                    UiWindowsManager.Instance.SeWindow.Toggle();
                }
                else if (UiWindowsManager.Instance.TextspeedWindow.IsOpen)
                {
                    UiWindowsManager.Instance.TextspeedWindow.Toggle();
                }
                else if (UiWindowsManager.Instance.BGMWindow.IsOpen)
                {
                    UiWindowsManager.Instance.BGMWindow.Toggle();
                }
                else
                {
                    UiWindowsManager.Instance.settingWindow.Toggle();
                }
            }

            //For Test InGameEvents
            if (Input.GetKeyDown(KeyCode.T))
            {
                // Player.Instance.InGameEventManager.Occur(inGameEventData);
            }
        }
    }
}