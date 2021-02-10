using GuildMaster.Data; //for test
using GuildMaster.InGameEvents; //for test
using GuildMaster.Windows;
using GuildMaster.Windows.Inven;
using UnityEditor; //for test
using UnityEngine;

namespace GuildMaster
{
    public class GameInput : MonoBehaviour
    {
        private void Update()
        {
            //씬별로 일부 창을 안띄우는 코드 구현 필요

            if (Input.GetKeyDown(KeyCode.O)) //퀘스트 리스트
                UiWindowsManager.Instance.questListWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.I)) // 인벤토리
            {
                UiWindowsManager.Instance.inventoryWindow.Toggle();
            }
            if (Input.GetKeyDown(KeyCode.P)) //캐릭터
                UiWindowsManager.Instance.characterInspectWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.G)) //길드 정보
                UiWindowsManager.Instance.guildInspectWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.E)) //탐험 캐릭터 선택 창
                UiWindowsManager.Instance.ExplorationCharacterSelectingWindow.Toggle();
            if (Input.GetKeyDown(KeyCode.S)) //상점 윈도우
                UiWindowsManager.Instance.shopWindow.Toggle();
            //Settings 와 그 자식 파일들 
            if (Input.GetKeyDown(KeyCode.Escape)) //ESC 환경설정
            {
                UiWindowsManager.Instance.CloseSingleWindow();
            }
            
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UiWindowsManager.Instance.CloseSingleWindow();
            }

            // For Test
            if (Input.GetKeyDown(KeyCode.T))
            {
                UiWindowsManager.Instance.TownMapWindow.Toggle();
            }
        }
    }
}
