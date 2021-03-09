using GuildMaster.Tools;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public sealed class ItemIcon : MonoBehaviour
    {
        [field: SerializeField] public ItemStackView ItemStackView { get; private set; }

        [field: SerializeField]
        [CanBeNull]
        public PointerEnterExitForwarder PointerEnterExitEvents { get; private set; }

        [field: SerializeField] [CanBeNull] public Button Button { get; private set; }
        [field: SerializeField] [CanBeNull] public DragForwarder DragForwarder { get; private set; }

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
        
        private bool _interactable = true;
    }
}