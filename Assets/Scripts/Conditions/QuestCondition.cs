using System;
using GuildMaster.Data;

namespace GuildMaster.Conditions
{
    public abstract class QuestCondition
    {
        private QuestCondition() {}
        public abstract Condition GenerateCompletionCondition(PlayerData playerData);
        [Serializable]
        public class StaticCondition : QuestCondition
        {
            public Condition condition;

            public StaticCondition(Condition condition)
            {
                this.condition = condition;
            }
            
            public override Condition GenerateCompletionCondition(PlayerData playerData)
            {
                return condition;
            }
        }
    }
}