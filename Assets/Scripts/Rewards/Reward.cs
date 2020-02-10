using System;
using GuildMaster.Npcs;

namespace GuildMaster.Rewards
{
    public class Reward
    {
        private Reward() {}

        [Serializable]
        public class AffinityReward : Reward
        {
            public NpcData targetNpc;
            public int amout;
        }
    }
}