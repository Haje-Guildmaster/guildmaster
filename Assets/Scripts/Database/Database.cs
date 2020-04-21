using System;
using System.Collections.Generic;
using GuildMaster.Items;
using UnityEngine;

namespace GuildMaster.Database
{
    [Serializable]
    public abstract class Database<TSelf, TElement> : ScriptableObject
    {
        [Serializable]
        public class Index
        {
            public int Value;
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
        where TIndex : Database<TSelf, TElement>.Index, new()
    {
        // DatabaseEditor을 위한 약간의 편법.
        public TIndex currentEditingIndex = new TIndex();
    }
}