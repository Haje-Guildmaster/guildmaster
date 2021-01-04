using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Exploration
{
    [RequireComponent(typeof(Toggle))]
    public class MapSelectViewToggle: MonoBehaviour
    {
        [field: SerializeField] public MapSelectView MapSelectView {get; private set; }

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.onValueChanged.AddListener(SetMapSelectViewActive);
        }

        private void SetMapSelectViewActive(bool active)
        {
            MapSelectView.gameObject.SetActive(active);
        }
        
        public bool IsOn
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }

        private Toggle _toggle;
    }
}