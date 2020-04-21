using System;
using System.Collections.Generic;
using GuildMaster.Items;
using UnityEngine;

namespace GuildMaster.Database
{
    [Serializable]
    public abstract class Database<TSelf, TElement> : ScriptableObject where TSelf: Database<TSelf, TElement>
    {
        [Serializable]
        public class Index
        {
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
        }

        public static TSelf Instance { get; private set; }

        public static void LoadSingleton(TSelf database)
        {
            Instance = database;
        }


        public TElement GetElement(Index index)
        {
            return dataList[index.Value];
        }

        public List<TElement> dataList;
    }

    [Serializable]
    public abstract class UnityEditableDatabase<TSelf, TElement, TIndex> : Database<TSelf, TElement>
        where TIndex : Database<TSelf, TElement>.Index, new() where TSelf: Database<TSelf, TElement>
    {
        // DatabaseEditor을 위한 약간의 편법.
        public TIndex currentEditingIndex = new TIndex();
    }
}