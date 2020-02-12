using GuildMaster.Events;
using GuildMaster.UI;
using UnityEngine;

namespace GuildMaster
{
    public class GameInput: MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                UiWindowsManager.Instance.ToggleQuestListWindow();

            //For Test InGameEvents
            if (Input.GetKeyDown(KeyCode.T))
                GameEvents.InGameEventManagerEventOccur.Invoke();
        }
    }
}