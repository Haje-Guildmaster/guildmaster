using System;

namespace GuildMaster.Exploration.Events
{
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
            public CharacterTraitData.Trait Trait;
        }
    }
}