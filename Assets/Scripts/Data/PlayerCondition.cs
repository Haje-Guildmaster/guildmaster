using System;
using GuildMaster.Databases;
using GuildMaster.GuildManagement;
using UnityEngine;

namespace GuildMaster.Data
{
    public partial class Player
    {
        [Serializable]
            public class Condition
            {
                private Condition() {}
        
                [Serializable]
                public class Always : Condition
                {
                    public bool isTrue;
        
                    public Always() {}
                    public Always(bool isTrue)
                    {
                        this.isTrue = isTrue;
                    }
                }
                [Serializable]
                public class GuildRankEqualOrOver : Condition
                {
                    public Guild.GuildRank rank;
        
                    public GuildRankEqualOrOver() {}
        
                    public GuildRankEqualOrOver(Guild.GuildRank rank)
                    {
                        this.rank = rank;
                    }
                }
        
                [Serializable]
                public class CompletedQuest : Condition
                {
                    public QuestCode questCode;
        
                    public CompletedQuest() {}
        
                    public CompletedQuest(QuestCode questCode)
                    {
                        this.questCode = questCode;
                    }
                }
        
                [Serializable]
                public class And : Condition
                {
                    [SerializeReference] [SerializeReferenceButton] public Condition[] conditions;
        
        
                    public And() {}
        
                    public And(params Condition[] list)
                    {
                        conditions = list;
                    }
                }
        
                [Serializable]
                public class Or: Condition
                {
                    public Condition[] conditions;
                    
                    public Or(params Condition[] list)
                    {
                        conditions = list;
                    }
                }
            }
    }
}