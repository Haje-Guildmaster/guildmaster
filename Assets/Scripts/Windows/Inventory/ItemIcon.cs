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


        private void Awake()
        {
            PointerEnterExitEvents = GetComponent<PointerEnterExitForwarder>();
            Button = GetComponent<Button>();
            DragForwarder = GetComponent<DragForwarder>();
        }
    }
}