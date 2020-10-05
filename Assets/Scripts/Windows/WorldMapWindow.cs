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
        Debug.Log("탐색 시작 와!");
        //탐색 시작 시 호출할 함수 넣으면 됨
    }

    public void Back()
    {
        base.Close();
        UiWindowsManager.Instance.ExplorationItemSelectingWindow.Toggle();
    }
}
