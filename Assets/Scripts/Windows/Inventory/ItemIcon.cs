using GuildMaster.Tools;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class ItemIcon : MonoBehaviour
    {
        [field: SerializeField] public ItemStackView ItemStackView { get; private set; }

        [field: SerializeField]
        [CanBeNull]
        public PointerEnterExitForwarder PointerEnterExitEvents { get; private set; }

        [field: SerializeField] [CanBeNull] public Button Button { get; private set; }
        [field: SerializeField] [CanBeNull] public DragForwarder DragForwarder { get; private set; }
        [field: SerializeField] public Image BackGroundImage { get; private set; }
        
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                if (Button != null) Button.interactable = _interactable;
                if (DragForwarder != null) DragForwarder.enabled = _interactable;
                if (PointerEnterExitEvents != null) PointerEnterExitEvents.enabled = _interactable;
            }
        }
        
        private CanvasGroup _canvasGroup;
        private bool _interactable = true;
    }
}