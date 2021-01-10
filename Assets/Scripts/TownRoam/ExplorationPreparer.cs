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
    public class ExplorationPreparer
    {
        public static ExplorationPreparer Instance => _instance = _instance ?? new ExplorationPreparer();

        public bool Running => _goExploreSingularRun.Running;

        public async void GoExplore()
        {
            await _goExploreSingularRun.Run(async cancellationToken =>
            {
                try
                {
                    List<Character> characters = new List<Character>();
                    var inventory = new Inventory(12, true);

                    var step = 0;
                    while (true)
                    {
                        switch (step)
                        {
                            case 0:
                            {
                                var characterResponse =
                                    await CharacterSelector.GetResponse(characters, cancellationToken);
                                characters = characterResponse.SelectedCharacters;
                                var next = characterResponse.NextAction;
                                if (next == ExplorationCharacterSelectingWindow.Response.ActionEnum.GoNext)
                                    step += 1;
                                else
                                    return;
                                break;
                            }
                            case 1:
                            {
                                var itemResponse = await ItemSelector.GetResponse(inventory, cancellationToken);
                                var next = itemResponse.NextAction;
                                if (next == ExplorationItemSelectingWindow.Response.ActionEnum.GoNext)
                                    step += 1;
                                else if (next == ExplorationItemSelectingWindow.Response.ActionEnum.GoBack)
                                    step -= 1;
                                else
                                    return;
                                break;
                            }
                            case 2:
                            {
                                var worldMapResponse = await WorldMapWindow.GetResponse(cancellationToken);
                                var next = worldMapResponse.NextAction;
                                if (next == WorldMapWindow.Response.ActionEnum.GoNext)
                                    step += 1;
                                else if (next == WorldMapWindow.Response.ActionEnum.GoBack)
                                    step -= 1;
                                else
                                    return;
                                break;
                            }
                            case 3:
                                if (characters.Count == 0)
                                {
                                    await UiWindowsManager.Instance
                                        .AsyncShowMessageBox("알림", "캐릭터를 1명 이상 선택해야 합니다.", new[] {"확인"});
                                    step = 0;
                                    break;
                                }

                                ExplorationLoader.Load(characters, inventory);
                                return;
                            default:
                                throw new Exception();
                        }
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

        private CancellationTokenSource _goExploreCancelTokenSource = new CancellationTokenSource();

        private readonly SingularRun _goExploreSingularRun = new SingularRun();

        private ExplorationCharacterSelectingWindow CharacterSelector =>
            UiWindowsManager.Instance.ExplorationCharacterSelectingWindow;

        private ExplorationItemSelectingWindow ItemSelector =>
            UiWindowsManager.Instance.ExplorationItemSelectingWindow;

        private WorldMapWindow WorldMapWindow => UiWindowsManager.Instance.worldMapWindow;

        private static ExplorationPreparer _instance;
    }
}