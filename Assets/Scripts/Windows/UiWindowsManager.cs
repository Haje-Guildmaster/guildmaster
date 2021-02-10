using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Windows.Inven;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class UiWindowsManager : MonoBehaviour
    {
        public NpcInteractWindow npcInteractWindow;
        public QuestSuggestWindow questSuggestWindow;
        public QuestListWindow questListWindow;
        public QuestInspectWindow questInspectWindow;
        public InGameEventWindow inGameEventWindow;
        public InventoryWindow inventoryWindow;
        public MessageBox messageBoxPrefab;
        public Transform messageBoxesParent;
        public CharacterInspectWindow characterInspectWindow;
        public GuildInspectWindow guildInspectWindow;
        public ShopWindow shopWindow;
        public TownMapWindow TownMapWindow;
        
        public SettingWindow settingWindow;
        public TextureWindow TextureWindow;
        public MVWindow MvWindow;
        public ResolutionWindow ResolutionWindow;
        public SEWindow SeWindow;
        public TextspeedWindow TextspeedWindow;
        public BGMWindow BGMWindow;
        public ExplorationCharacterSelectingWindow ExplorationCharacterSelectingWindow;
        public ExplorationItemSelectingWindow ExplorationItemSelectingWindow;
        public WorldMapWindow worldMapWindow;

        public ItemInfoPanel itemInfoPanel;    // 임시.

        /// <summary>
        /// 메시지 창을 띄움
        /// </summary>
        /// <param name="title"> 메시지 창 제목 </param>
        /// <param name="content"> 메시지 창 내용 </param>
        /// <param name="buttons"> 각 버튼의 (안에 들어갈 텍스트, 클릭시의 행동) 의 리스트 </param>
        public void ShowMessageBox(string title, string content, IEnumerable<(string buttonText, Action onClicked)> buttons)
        {
            var messageBox = Instantiate(messageBoxPrefab, messageBoxesParent);
            messageBox.Open(title, content, buttons);
        }

        /// <summary>
        /// 비동기적으로 메시지 창을 띄우고 선택된 버튼의 인덱스 값을 반환함.
        /// </summary>
        /// <param name="title"> 메시지 창 제목 </param>
        /// <param name="content"> 메시지 창 내용 </param>
        /// <param name="buttonTexts"> 각 버튼 안에 들어갈 텍스트 리스트 </param>
        /// <returns> 선택된 버튼의 인덱스 값 </returns>
        public async Task<int> AsyncShowMessageBox(string title, string content, IEnumerable<string> buttonTexts)
        {
            var tcs = new TaskCompletionSource<int>();
            ShowMessageBox(title, content, buttonTexts.Select<string, (string, Action)>((text, i) => (text, () => tcs.SetResult(i))));
            return await tcs.Task;
        }

        public void CloseSingleWindow()
        {
            if (worldMapWindow.IsOpen)
            {
                worldMapWindow.Close();
            }
            else if (ExplorationCharacterSelectingWindow.IsOpen)
            {
                ExplorationCharacterSelectingWindow.Close();
            }
            else if (ExplorationItemSelectingWindow.IsOpen)
            {
                ExplorationItemSelectingWindow.Close();
            }
            else if (questListWindow.IsOpen)
            {
                questListWindow.Close();
            }
            else if (inventoryWindow.IsOpen)
            {
                inventoryWindow.Close();
            }
            else if (characterInspectWindow.IsOpen)
            {
                characterInspectWindow.Close();
            }
            else if (guildInspectWindow.IsOpen)
            {
                guildInspectWindow.Close();
            }
            else if (shopWindow.IsOpen)
            {
                shopWindow.Close();
            }
            else if (TextureWindow.IsOpen)
            {
                TextureWindow.Toggle();
            }
            else if (MvWindow.IsOpen)
            {
                MvWindow.Toggle();
            }
            else if (ResolutionWindow.IsOpen)
            {
                ResolutionWindow.Toggle();
            }
            else if (SeWindow.IsOpen)
            {
                SeWindow.Toggle();
            }
            else if (TextspeedWindow.IsOpen)
            {
                TextspeedWindow.Toggle();
            }
            else if (BGMWindow.IsOpen)
            {
                BGMWindow.Toggle();
            }
            else
            {
                settingWindow.Toggle();
            }
            return;
        }

        private static UiWindowsManager _instance;
        public static UiWindowsManager Instance =>
            _instance != null ? _instance : (_instance = FindObjectOfType<UiWindowsManager>());
    }
}
