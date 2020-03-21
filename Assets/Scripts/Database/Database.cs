using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.Database
{
    public interface IDatabase<in TIndex, out TElement> where TIndex: DatabaseIndex
    {
        TElement GetElement(TIndex index);
    }
    
    [Serializable]
    public abstract class Database<TIndex, TElement>: ScriptableObject, IDatabase<TIndex, TElement> where TIndex: DatabaseIndex
    {
        public TElement GetElement(TIndex index)
        {
            return dataList[index.Value];
        }

        public List<TElement> dataList;
    }

    [Serializable]
    public abstract class EditableDatabase<TIndex, TElement> : Database<TIndex, TElement>
        where TIndex : DatabaseIndex, new()
    {
    // DatabaseEditor을 위한 약간의 편법.
    public TIndex currentEditingIndex = new TIndex();
    }
}