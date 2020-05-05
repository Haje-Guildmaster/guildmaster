using System.Collections.Generic;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Rewards;
using UnityEngine;

namespace GuildMaster.Testing
{
    // Start 이벤트에 길드 멤버들 추가
    public class GiveGuildMembers: MonoBehaviour
    {
        public List<CharacterData> characters;

        private void Start()
        {
            foreach (var ch in characters)
                PlayerData.Instance.PlayerGuild._guildMembers.guildMemberList.Add(ch);
            Destroy(this);
        }
    }
}