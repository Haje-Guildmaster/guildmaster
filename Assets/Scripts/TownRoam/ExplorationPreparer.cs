using System;
using System.Collections.Generic;
using System.Threading;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Exploration;
using GuildMaster.Windows;

namespace GuildMaster.TownRoam
{
    public class ExplorationPreparer
    {
        public static ExplorationPreparer Instance => _instance = _instance ?? new ExplorationPreparer();

        public bool Running { get; private set; } = false;

        public async void GoExplore()
        {
            if (Running)
                throw new InvalidOperationException("Cannot run two preparation at once");
            Running = true;


            List<Character> characters = null;
            var inventory = new Inventory(12, true);

            var step = 0;
            while (true)
            {
                switch (step)
                {
                    case 0:
                    {
                        var characterResponse = await CharacterSelector.GetResponse();
                        characters = characterResponse.SelectedCharacters;
                        var next = characterResponse.NextAction;
                        if (next == ExplorationCharacterSelectingWindow.Response.ActionEnum.GoNext)
                            step += 1;
                        else
                            goto end;
                        break;
                    }
                    case 1:
                    {
                        var itemResponse = await ItemSelector.GetResponse(inventory);
                        var next = itemResponse.NextAction;
                        if (next == ExplorationItemSelectingWindow.Response.ActionEnum.GoNext)
                            step += 1;
                        else if (next == ExplorationItemSelectingWindow.Response.ActionEnum.GoBack)
                            step -= 1;
                        else
                            goto end;
                        break;
                    }
                    case 2:
                    {
                        var worldMapResponse = await WorldMapWindow.GetResponse();
                        var next = worldMapResponse.NextAction;
                        if (next == WorldMapWindow.Response.ActionEnum.GoNext)
                            step += 1;
                        else if (next == WorldMapWindow.Response.ActionEnum.GoBack)
                            step -= 1;
                        else
                            goto end;
                        break;
                    }
                    case 3:
                        ExplorationLoader.Load(characters, inventory);
                        goto end;
                    default:
                        throw new Exception();
                }
            }
            end:
            
            Running = false;
        }

        public void Reset()
        {
            CharacterSelector.Close();
            ItemSelector.Close();
            WorldMapWindow.Close();
            // _goExploreCancelSource = new CancellationTokenSource();
        }


        // private CancellationTokenSource _goExploreCancelSource = new CancellationTokenSource();

        private ExplorationCharacterSelectingWindow CharacterSelector =>
            UiWindowsManager.Instance.ExplorationCharacterSelectingWindow;

        private ExplorationItemSelectingWindow ItemSelector =>
            UiWindowsManager.Instance.ExplorationItemSelectingWindow;

        private WorldMapWindow WorldMapWindow => UiWindowsManager.Instance.worldMapWindow;

        private static ExplorationPreparer _instance;
    }
}