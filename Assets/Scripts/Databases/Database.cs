using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace GuildMaster.Databases
{
    [Serializable]
    public abstract class Database<TSelf, TElement> : ScriptableObject where TSelf : Database<TSelf, TElement>
    {
        protected static TSelf Instance;

        public static void LoadSingleton(TSelf database)
        {
            if (Instance != null)
                throw new Exception("This function should be called only one time");
            Instance = database;
        }
        
        protected TElement _GetElementByInt(int index)
        {
            return dataList.ElementAtOrDefault(index);
        }
        public IEnumerable<(int, TElement)> GetAllElements()
        {
            return dataList.Select((el,i)=>(i, el));
        }
        [SerializeField] private List<TElement> dataList = new List<TElement>();
    }
}