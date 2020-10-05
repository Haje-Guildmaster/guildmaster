using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace GuildMaster.Databases
{
    /// <summary>
    /// 이름은 데이터베이스이나 실상은 싱글톤 배열입니다. 게임 내에서 바뀌지 않는 값들을 저장하는 데에 사용합니다.
    /// </summary>
    /// <typeparam name="TSelf"> 자기 자신의 타입 </typeparam>
    /// <typeparam name="TElement"> 원소의 타입 </typeparam>
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