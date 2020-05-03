using System;
using GuildMaster.Database;
using GuildMaster.Items;
using GuildMaster.Npcs;

namespace GuildMaster.Rewards
{
    [Serializable]
    public class Reward
    {
        private Reward() {}

        [Serializable]
        public class AffinityReward : Reward
        {
            public NpcCode targetNpc;
            public int amount;
        }

        [Serializable]
        public class ItemReward : Reward
        {
            public Item item;
            public int number;
        }
    }
}