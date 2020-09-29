using System;
using GuildMaster.Tools;
using UnityEditorInternal;

namespace GuildMaster.GuildManagement
{
    /// <summary>
    /// 길드에 대한 정보를 저장하는 데이터 클래스.
    /// </summary>
    public class Guild
    {
        public event Action Changed;

        public Guild()
        {
            void InvokeChanged() => Changed?.Invoke();
            Rank.Changed += InvokeChanged;
            MemberNumberLimit.Changed += InvokeChanged;
            Balance.Changed += InvokeChanged;
            Reputation.Changed += InvokeChanged;
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