using System;
using GuildMaster.TownRoam;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    [RequireComponent(typeof(Button))]
    public class MovePlaceButton: MonoBehaviour
    {
        [SerializeField] private PlaceName _placeName;
        public event Action BeforeMoving;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(MovePlace);
        }

        private void MovePlace()
        {
            BeforeMoving?.Invoke();
            TownRoamManager.Instance.GotoPlace(_placeName);
        }

        private Button _button;
    }
}