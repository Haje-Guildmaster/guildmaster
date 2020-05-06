using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Database;
using GuildMaster.Rewards;
using UnityEngine;

namespace GuildMaster.Testing
{
    // Start 이벤트에 길드 멤버들 추가
    public class GiveGuildMembers: MonoBehaviour
    {
        public List<CharacterCode> characters;

        private void Start()
        {
            foreach (var chCode in characters)
                Player.Instance.PlayerGuild._guildMembers.GuildMemberList.Add(new Character(chCode));
            Destroy(this);
        }
    }
}