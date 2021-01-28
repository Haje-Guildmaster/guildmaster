using System;
using UnityEngine;

namespace GuildMaster.Tools
{
    /// <summary>
    /// interface T를 지닌 유니티 오브젝트에 대한 reference를 unity inspector에서 지정하기 위해 사용하는 클래스. <br/>
    /// interface T 멤버가 serialized 되야 할 경우, 이 클래스를 대신 멤버로 넣은 뒤 .Object 로 진짜 오브젝트에 접근하면 됨. <br/>
    /// 유니티 오브젝트(UnityEngine.Object를 상속한 클래스들)에 대한 링크만 가능합니다. <br/>
    /// inspector에서 GameObject를 끌어오는 걸로는 연결이 안 되니 inspector 두 개 띄워두고 컴포넌트를 직접 끌어 와야 함.
    /// </summary>
    /// <typeparam name="T"> 이 오브젝트가 지닌 유니티 오브젝트에 대한 reference가 지니는 타입. </typeparam>
    [Serializable]
    public class ObjectWith<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField] private UnityEngine.Object _innerValue;
        public T Object { get; private set; }


        public void OnBeforeSerialize()
        {
            _innerValue = Object as UnityEngine.Object;
        }

        public void OnAfterDeserialize()
        {
            Object = _innerValue as T;
            // if (Object == null && _innerValue != null)
                // throw new Exception($"_innerValue should be of type '{typeof(T)}'");
        }
    }
}