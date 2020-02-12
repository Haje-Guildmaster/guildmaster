using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Data
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
        public class LevelOver : Condition
        {
            public int level;

            public LevelOver() {}

            public LevelOver(int level)
            {
                this.level = level;
            }
        }

        [Serializable]
        public class CompletedQuest : Condition
        {
            public QuestData quest;

            public CompletedQuest() {}

            public CompletedQuest(QuestData quest)
            {
                this.quest = quest;
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