using GuildMaster.UI;
using UnityEngine;

namespace GuildMaster
{
    public class GameInput: MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
                UiWindowsManager.Instance.ToggleQuestListWindow();
        }
    }
}