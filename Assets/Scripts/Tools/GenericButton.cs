using System;
using GuildMaster.TownRoam;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GuildMaster.Tools
{
    /// <summary>
    /// 클릭당했을 시 EventArgument를 인수로 Clicked이벤트를 발생시킵니다.
    /// </summary>
    /// <typeparam name="T"> EventArgument의 타입. </typeparam>
    public abstract class GenericButton<T>: ClickableComponent
    {
        public delegate void ClickedHandler(T param);

        public event ClickedHandler Clicked;

        protected sealed override void OnClick()
        {
            Clicked?.Invoke(EventArgument);
        }
        
        protected abstract T EventArgument { get; }
    }
}