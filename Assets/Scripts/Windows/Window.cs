using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public abstract class Window: MonoBehaviour, IPointerDownHandler
    {
        public event Action Closed;
        
        protected void OpenWindow()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public void Close()
        {
            OnClose();
            Closed?.Invoke();
            gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }

        public bool IsOpen => gameObject.activeSelf;
        
        protected virtual void OnClose(){}
    }
}