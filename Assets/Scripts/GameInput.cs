using GuildMaster.Data; //for test
using GuildMaster.InGameEvents; //for test
using GuildMaster.Windows;
using UnityEditor; //for test
using UnityEngine;

namespace GuildMaster
{
    public class GameInput: MonoBehaviour
    {
        private void Update()
        {
            //씬별로 일부 창을 안띄우는 코드 구현 필요

            if (Input.GetKeyDown(KeyCode.E))
                UiWindowsManager.Instance.ExplorationCharacterSelectingWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.O)) //퀘스트 리스트
                UiWindowsManager.Instance.questListWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.I)) //인벤토리
                UiWindowsManager.Instance.inventoryWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.P)) //캐릭터
                UiWindowsManager.Instance.characterInspectWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.G)) //길드 정보
                UiWindowsManager.Instance .guildInspectWindow.Toggle();
            //Settings 와 그 자식 파일들 
            if (Input.GetKeyDown(KeyCode.Escape)) //ESC 환경설정
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