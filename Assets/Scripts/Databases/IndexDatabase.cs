using System;
using System.Runtime.InteropServices;
using GuildMaster.Characters;

namespace GuildMaster.Databases
{
    [Serializable]
    public abstract class IndexDatabase<TSelf, TElement> : Database<TSelf, TElement>
        where TSelf : IndexDatabase<TSelf, TElement>
    {
        [Serializable]
        public class Index
        {
            public Index() : this(0)
            {
            }

            public Index(int ind)
            {
                Value = ind;
            }

            public int Value;

            public static bool operator ==(Index i1, Index i2)
            {
                return i1?.Equals(i2) ?? ReferenceEquals(i2, null);
            }

            public static bool operator !=(Index i1, Index i2)
            {
                return !(i1 == i2);
            }

            protected bool Equals(Index other)
            {
                return Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Index) obj);
            }

            public override int GetHashCode()
            {
                return Value;
            }
        }

        public TElement _GetElement(Index index) => base._GetElementByInt(index.Value);
        /// <summary>
        /// DB에서 인덱스를 이용해 element 하나를 가져옵니다. 인덱스가 범위를 벗어난다면 null을 반환합니다.
        /// </summary>
        /// <param name="index"> 인덱스 </param>
        /// <returns> element at the index </returns>
        public static TElement Get(Index index) => Instance._GetElement(index); // 디비 접근시마다 Instance쓰는 게 싫어서 추가.
    }

    /// <summary>
    /// DatabaseEditor에서 편집할 수 있도록 한 IndexDatabase입니다.
    /// </summary>
    /// <typeparam name="TSelf"> 자기 자신의 타입 </typeparam>
    /// <typeparam name="TElement"> DB 원소 타입 </typeparam>
    [Serializable]
    public abstract class UnityEditableIndexDatabase<TSelf, TElement> : IndexDatabase<TSelf, TElement>
        where TSelf : IndexDatabase<TSelf, TElement>
    {
        // DatabaseEditor을 위한 약간의 편법.
        public Index currentEditingIndex = new Index();
    }
}