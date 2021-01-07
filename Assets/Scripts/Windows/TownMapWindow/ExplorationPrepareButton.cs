using System;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    [RequireComponent(typeof(Button))]
    public class ExplorationPrepareButton: MonoBehaviour
    {
        public event Action BeforeAction;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(StartExplorationPreparation);
        }

        private void StartExplorationPreparation()
        {
            BeforeAction?.Invoke();
            // UiWindowsManager.Instance.ExplorationCharacterSelectingWindow.Open();
        }

        private Button _button;
    }
}