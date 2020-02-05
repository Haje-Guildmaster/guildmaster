using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public abstract class Window: MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent closed = new UnityEvent();
        
        protected void Awake()
        {
            gameObject.SetActive(false);
        }
        
        public void Open()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        public void Close()
        {
            OnClose();
            closed.Invoke();
            gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }

        protected virtual void OnOpen(){}

        protected virtual void OnClose(){}
    }
}