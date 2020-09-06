using System;

namespace GuildMaster.Databases
{
    public class EnumDatabase<TSelf, TElement, TIndexEnum> : Database<TSelf, TElement>
        where TSelf : EnumDatabase<TSelf, TElement, TIndexEnum>
        where TIndexEnum : Enum
    {
        public TElement _GetElement(TIndexEnum indexEnum) => _GetElementByInt(Convert.ToInt32(indexEnum));
        public TElement Get(TIndexEnum indexEnum) => Instance._GetElement(indexEnum);
    }

    public class UnityEditableEnumDatabase<TSelf, TElement, TIndexEnum> : EnumDatabase<TSelf, TElement, TIndexEnum>
        where TSelf : EnumDatabase<TSelf, TElement, TIndexEnum>
        where TIndexEnum : Enum, new()
    {
        public TIndexEnum currentEditingIndex = new TIndexEnum();
    }
}