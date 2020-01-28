using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildMaster.Quests;
using UnityEngine;

namespace GuildMaster.Conditions
{
    [Serializable]
    public class Condition
    {
        private Condition()
        {
        }

        [Serializable]
        public class LevelOver : Condition
        {
            public int level;

            public LevelOver(int level)
            {
                this.level = level;
            }
        }

        [Serializable]
        public class CompletedQuest : Condition
        {
            public QuestData quest;

            public CompletedQuest(QuestData quest)
            {
                this.quest = quest;
            }
        }

        [Serializable]
        public class And: Condition
        {
            public Condition[] conditions;

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