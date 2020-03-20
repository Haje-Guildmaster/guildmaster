using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.Tools
{
    public interface IDatabase<in TEnum, out TElement> where TEnum : Enum
    {
        TElement GetElement(TEnum code);
    }
    
    [Serializable]
    public abstract class Database<TEnum, TElement>: ScriptableObject, IDatabase<TEnum, TElement> where TEnum: Enum
    {
        public TElement GetElement(TEnum code)
        {
            return dataList[Convert.ToInt32(code)];
        }

        public List<TElement> dataList;
    }
}