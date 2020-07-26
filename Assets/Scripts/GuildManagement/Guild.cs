using System;
using GuildMaster.Tools;
using UnityEditorInternal;

namespace GuildMaster.GuildManagement
{
    public class Guild
    {
        public event Action Changed;

        public Guild()
        {
            Rank.Changed += Changed;
            MemberNumberLimit.Changed += Changed;
            Balance.Changed += Changed;
            Reputation.Changed += Changed;
        }

        public enum GuildRank
        {
            D, C, B, A
        }

        public readonly ChangeTrackedValue<GuildRank> Rank = new ChangeTrackedValue<GuildRank>(GuildRank.D);

        public readonly ChangeTrackedValue<int> MemberNumberLimit = new ChangeTrackedValue<int>(5);
        public readonly ChangeTrackedValue<int> Balance = new ChangeTrackedValue<int>(4231);
        public readonly ChangeTrackedValue<int> Reputation = new ChangeTrackedValue<int>(0);

        /*private*/
        public GuildMembers _guildMembers = new GuildMembers();
    }
}