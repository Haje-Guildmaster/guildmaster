﻿using GuildMaster.Data; //for test
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

            //For Test InGameEvents
            if (Input.GetKeyDown(KeyCode.T))
            {
                InGameEventData inGameEventData = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/ScriptableObjects/InGameEvent/CursedOldBook.asset") as InGameEventData;
                PlayerData.Instance.InGameEventManager.Occur(inGameEventData);
                
            }
        }
    }
}