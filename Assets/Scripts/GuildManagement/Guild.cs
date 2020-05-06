using System;

namespace GuildMaster.GuildManagement
{
    public class Guild
    {
        public event Action Changed;    // 미완.

        public enum GuildRank
        {
            D, C, B, A
        }

        public GuildRank Rank { get; private set; } = GuildRank.D;
        public int MemberNumberLimit { get; private set; } = 5;
        public int Balance { get; private set; } = 4231;
        public int Reputation { get; private set; } = 0;

        /*private*/ public GuildMembers _guildMembers = new GuildMembers();
    }
}