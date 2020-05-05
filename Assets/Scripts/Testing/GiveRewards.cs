using System;
using System.Collections.Generic;
using GuildMaster.Data;
using GuildMaster.Rewards;
using UnityEngine;

namespace GuildMaster.Testing
{
    // 디버깅용. 게임 시작 시 reward 들을 적용.
    public class GiveRewards: MonoBehaviour
    {
        [SerializeReference][SerializeReferenceButton] public List<Reward> rewards;

        private void Start()
        {
            foreach (var reward in rewards)
                Player.Instance.ApplyReward(reward);
            Destroy(this);
        }
    }
}