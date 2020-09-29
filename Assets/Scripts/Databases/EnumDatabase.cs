using System;

namespace GuildMaster.Databases
{
    /// <summary>
    /// Enum을 인덱스로 사용하는 DB(배열)입니다.
    /// </summary>
    /// <typeparam name="TSelf"> 자기 자신의 타입 </typeparam>
    /// <typeparam name="TElement"> DB 원소 타입 </typeparam>
    /// <typeparam name="TIndexEnum"> 인덱스 enum 타입 </typeparam>
    public class EnumDatabase<TSelf, TElement, TIndexEnum> : Database<TSelf, TElement>
        where TSelf : EnumDatabase<TSelf, TElement, TIndexEnum>
        where TIndexEnum : Enum
    {
        public TElement _GetElement(TIndexEnum indexEnum) => _GetElementByInt(Convert.ToInt32(indexEnum));
        public static TElement Get(TIndexEnum indexEnum) => Instance._GetElement(indexEnum);
    }
    
    /// <summary>
    /// DatabaseEditor에서 편집할 수 있도록 한 IndexDatabase입니다.
    /// </summary>
    /// <typeparam name="TSelf"> 자기 자신의 타입 </typeparam>
    /// <typeparam name="TElement"> DB 원소 타입 </typeparam>
    /// <typeparam name="TIndexEnum"> 인덱스 enum 타입 </typeparam>
    public class UnityEditableEnumDatabase<TSelf, TElement, TIndexEnum> : EnumDatabase<TSelf, TElement, TIndexEnum>
        where TSelf : EnumDatabase<TSelf, TElement, TIndexEnum>
        where TIndexEnum : Enum, new()
    {
        public TIndexEnum currentEditingIndex = new TIndexEnum();
    }
}