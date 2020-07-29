using System;
using UnityEditorInternal;

namespace GuildMaster.Tools
{
    /// <summary>
    /// <c>T</c>타입 변수 하나를 저장하며, 그 변수의 변화를 탐지합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChangeTrackedValue<T>
    {
        public event Action Changed;

        public ChangeTrackedValue()
        {
        }

        public ChangeTrackedValue(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke();
            }
        }


        public override string ToString()
        {
            return Value.ToString();
        }

        private T _value;
        public static implicit operator T(ChangeTrackedValue<T> ctv) => ctv.Value;
    }
}