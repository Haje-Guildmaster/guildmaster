using GuildMaster.Exploration;
using GuildMaster.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapWindow : DraggableWindow, IToggleableWindow
{
    public void Open()
    {
        base.OpenWindow();
    }

    public void GoToExploration()
    {
        base.Close();
        ExplorationLoader.Load(UiWindowsManager.Instance.ExplorationCharacterSelectingWindow.exploreCharacterList);
    }

    public void Back()
    {
        base.Close();
        UiWindowsManager.Instance.ExplorationItemSelectingWindow.Toggle();
    }
}
