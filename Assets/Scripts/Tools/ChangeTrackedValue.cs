using System;
using UnityEditorInternal;
using UnityEngine;

namespace GuildMaster.Tools
{
    /// <summary>
    /// <c>T</c>타입 변수 하나를 저장하며, 그 변수의 변화를 탐지합니다.
    /// 안의 값을 바꿀려면 .Value를 바꿔 주세요. 값을 사용하는 경우에는 그냥 T를 사용하듯이 써도 되고, .Value를 사용해도 됩니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ChangeTrackedValue<T>: IReadOnlyChangeTrackedValue<T>
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

        [SerializeField] private T _value;
        public static implicit operator T(ChangeTrackedValue<T> ctv) => ctv.Value;
    }
}