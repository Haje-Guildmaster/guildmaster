using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.Tools
{
    public class PointerEnterExitForwarder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> PointerEnter;
        public event Action<PointerEventData> PointerExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit?.Invoke(eventData);
        }
    }
}