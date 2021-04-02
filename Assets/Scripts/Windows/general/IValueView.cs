using System;
using UnityEngine;

namespace GuildMaster.Windows
{
    /// <summary>
    /// T를 보여주는 유니티 오브젝트. <br/>
    /// interface 대신 monobehaviour을 상속한 abstract class인 건 이래야 유니티 인스펙터에서 오브젝트 연결로 떠서.
    /// </summary>
    /// <note>
    /// </note>
    /// <typeparam name="T"></typeparam>
    public interface IValueView<in T>
    {
        void SetValue(T value);
    }
}