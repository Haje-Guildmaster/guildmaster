using System;
using System.Collections.Generic;
using System.Threading;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Exploration;
using GuildMaster.Tools;
using GuildMaster.Windows;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 탐색을 준비를 총괄합니다. 탐색에 필요한 캐릭터 선택, 아이템 선택, 맵 선택을 마치고 탐색을 시작시킵니다. 
    /// </summary>
    public class ExplorationPreparer
    {
        public static ExplorationPreparer Instance => _instance = _instance ?? new ExplorationPreparer();

        public bool Running => _goExploreSingularRun.Running;

        /// <summary>
        /// 탐색 준비를 시작합니다. 탐색에 필요한 캐릭터 선택, 아이템 선택, 맵 선택을 마치고 탐색을 시작시킵니다.
        /// </summary>
        public async void GoExplore()
        {
            await _goExploreSingularRun.Run(async cancellationToken =>
            {
                try
                {
                    var characters = new List<Character>();
                    var inventory = new Inventory(12, true);

                    switch (Step.CharacterSelect)
                    {
                        case Step.CharacterSelect:
                        {
                            var characterResponse =
                                await CharacterSelector.GetResponse(characters, cancellationToken);
                            characters = characterResponse.SelectedCharacters;
                            var next = characterResponse.NextAction;
                            if (next == ExplorationCharacterSelectingWindow.Response.ActionEnum.GoNext)
                                goto case Step.ItemSelect;
                            return;
                        }
                        case Step.ItemSelect:
                        {
                            var itemResponse = await ItemSelector.GetResponse(inventory, cancellationToken);
                            var next = itemResponse.NextAction;
                            if (next == ItemSelectingWindow.Response.ActionEnum.GoNext)
                                goto case Step.MapSelect;
                            if (next == ItemSelectingWindow.Response.ActionEnum.GoBack)
                                goto case Step.CharacterSelect;
                            return;
                        }
                        case Step.MapSelect:
                        {
                            var worldMapResponse = await WorldMapWindow.GetResponse(cancellationToken);
                            var next = worldMapResponse.NextAction;
                            if (next == WorldMapWindow.Response.ActionEnum.GoNext)
                                goto case Step.CallExplorationLoader;
                            if (next == WorldMapWindow.Response.ActionEnum.GoBack)
                                goto case Step.ItemSelect;
                            return;
                        }
                        case Step.CallExplorationLoader:
                            if (characters.Count == 0)
                            {
                                await UiWindowsManager.Instance
                                    .AsyncShowMessageBox("알림", "캐릭터를 1명 이상 선택해야 합니다.", new[] {"확인"});
                                goto case Step.CharacterSelect;
                            }

                            ExplorationLoader.Load(characters, inventory);
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Do nothing
                }
            }, _goExploreCancelTokenSource.Token);
        }

        public void Cancel()
        {
            _goExploreCancelTokenSource.Cancel();
            _goExploreCancelTokenSource = new CancellationTokenSource();
        }

        public void Toggle()
        {
            if (Running)
                Cancel();
            else
                GoExplore();
        }

        private enum Step
        {
            CharacterSelect, ItemSelect, MapSelect, CallExplorationLoader
        }
        
        private CancellationTokenSource _goExploreCancelTokenSource = new CancellationTokenSource();

        private readonly SingularRun _goExploreSingularRun = new SingularRun();

        private ExplorationCharacterSelectingWindow CharacterSelector =>
            UiWindowsManager.Instance.ExplorationCharacterSelectingWindow;

        private ItemSelectingWindow ItemSelector =>
            UiWindowsManager.Instance.ExplorationItemSelectingWindow;

        private WorldMapWindow WorldMapWindow => UiWindowsManager.Instance.worldMapWindow;

        private static ExplorationPreparer _instance;
    }
}