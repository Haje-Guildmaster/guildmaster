using System;
using GuildMaster.TownRoam;
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
            ExplorationPreparer.Instance.GoExplore();
        }

        private Button _button;
    }
}