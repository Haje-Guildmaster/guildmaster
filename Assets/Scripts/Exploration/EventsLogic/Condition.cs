using System;
using GuildMaster.Characters;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 이벤트에서 특정 조건을 나타내는 클래스.
    /// </summary>
    [Serializable]
    public abstract class Condition
    {
        private Condition(){}

        [Serializable]
        public class Always : Condition
        {
            public bool IsTrue;
        }

        [Serializable]
        public class HasTrait : Condition
        {
            public Trait Trait;
        }
    }
}