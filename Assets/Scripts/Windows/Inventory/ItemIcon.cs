using GuildMaster.Tools;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public sealed class ItemIcon : MonoBehaviour
    {
        [field: SerializeReference] public ItemStackView ItemStackView { get; private set; }

        [CanBeNull] public PointerEnterExitForwarder PointerEnterExitEvents { get; private set; }
        [CanBeNull] public Button Button { get; private set; }
        [CanBeNull] public DragForwarder DragForwarder { get; private set; }

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

        private void Awake()
        {
            PointerEnterExitEvents = GetComponent<PointerEnterExitForwarder>();
            Button = GetComponent<Button>();
            DragForwarder = GetComponent<DragForwarder>();
        }

        private bool _interactable = true;
    }
}