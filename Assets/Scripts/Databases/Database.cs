using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuildMaster.Databases
{
    [Serializable]
    public abstract class Database<TSelf, TElement> : ScriptableObject where TSelf: Database<TSelf, TElement>
    {
        [Serializable]
        public class Index
        {
            public Index() : this(0) {}

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

        private static TSelf _instance;

        public static void LoadSingleton(TSelf database)
        {
            if (_instance != null)
                throw new Exception("This function should be called only one time");
            _instance = database;
        }

        public static TElement Get(Index index) => _instance._GetElement(index);     // 디비 접근시마다 Instance쓰는 게 싫어서 추가.

        public IEnumerable<(int, TElement)> GetAllElements()
        {
            return dataList.Select((el,i)=>(i, el));
        }
        
        
        public TElement _GetElement(Index index)
        {
            return dataList.ElementAtOrDefault(index.Value);
        }
        
        
        [SerializeField] private List<TElement> dataList = new List<TElement>();
    }

    [Serializable]
    public abstract class UnityEditableDatabase<TSelf, TElement, TIndex> : Database<TSelf, TElement>
        where TIndex : Database<TSelf, TElement>.Index, new() where TSelf: Database<TSelf, TElement>
    {
        // DatabaseEditor을 위한 약간의 편법.
        public TIndex currentEditingIndex = new TIndex();
    }
}