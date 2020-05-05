using System;
using System.Linq;
using GuildMaster.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.UI
{
    public class GuildInspectWindow: DraggableWindow, IToggleableWindow
    {
        [SerializeField] private Text rankLabel;
        [SerializeField] private Text membersNumberLabel;
        [SerializeField] private Text balanceLabel;
        [SerializeField] private Text reputationLabel;

        public void OnEnable()
        {
            PlayerData.Instance.PlayerGuild.Changed += Refresh;
        }
        public void OnDisable()
        {
            PlayerData.Instance.PlayerGuild.Changed -= Refresh;
        }

        public void Open()
        {
            base.OpenWindow();
            Refresh();
        }

        private void Refresh()
        {
            var guild = PlayerData.Instance.PlayerGuild;
            rankLabel.text = guild.Rank.ToString();
            membersNumberLabel.text = $"{guild._guildMembers.guildMemberList.Count()}/{guild.MemberNumberLimit}";
            balanceLabel.text = guild.Balance.ToString();
            reputationLabel.text = guild.Reputation.ToString();
        }

    }
}