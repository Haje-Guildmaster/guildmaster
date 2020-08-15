using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Databases;
using GuildMaster.GuildManagement;
using GuildMaster.InGameEvents;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.Rewards;
using JetBrains.Annotations;

namespace GuildMaster.Data
{
    /// 플레이어의 플레이 정보를 담습니다.
    /// 퀘스트 클리어 정보, 길드원들, npc상태, 레벨, 장비, etc...
    public partial class Player
    {
        public static Player Instance
        {
            get { return _instance = _instance ?? new Player(); }
        }

        public event Action Changed;

        // 속한 데이터
        public readonly QuestManager QuestManager;
        public readonly Inventory Inventory;
        public readonly InGameEventManager InGameEventManager;
        public readonly Guild PlayerGuild;
        private readonly Dictionary<int, NpcStatus> _npcStatusMap = new Dictionary<int, NpcStatus>(); // array로 바꿀수도.

        // 

        [NotNull]
        public NpcStatus GetNpcStatus(NpcCode npc)
        {
            return _npcStatusMap.TryGetValue(npc.Value, out var ret) ? ret : _AddNpcStatus(npc);
        }

        public void ApplyReward(Reward reward)
        {
            switch (reward)
            {
                case Reward.AffinityReward affinityReward:
                    GetNpcStatus(affinityReward.targetNpc).Affinity.Value += affinityReward.amount;
                    break;
                case Reward.ItemReward itemReward:
                    Inventory.TryAddItem(itemReward.item, itemReward.number);
                    break;
                default:
                    throw new Exception($"Unexpected Reward: {reward}");
            }
        }

        public bool CheckCondition(Condition condition)
        {
            switch (condition)
            {
                case null:
                    return true;
                case Condition.Always always:
                    return always.isTrue;
                case Condition.And and:
                    return and.conditions.Aggregate(true, (calc, cond) => calc && CheckCondition(cond));
                case Condition.Or or:
                    return or.conditions.Aggregate(false, (calc, cond) => calc || CheckCondition(cond));
                case Condition.CompletedQuest hasCompletedQuest:
                    return QuestManager.CompletedQuest(hasCompletedQuest.questCode);
                case Condition.GuildRankEqualOrOver rankOver:
                    return rankOver.rank >= PlayerGuild.Rank;
                default:
                    throw new Exception($"Unexpected Condition: {condition}");
            }
        }


        [NotNull]
        private NpcStatus _AddNpcStatus(NpcCode npc)
        {
            var made = new NpcStatus();
            _npcStatusMap.Add(npc.Value, made);
            made.Changed += Changed;
            Changed?.Invoke();
            return made;
        }

        private static Player _instance;

        private Player()
        {
            QuestManager = new QuestManager(this);
            InGameEventManager = new InGameEventManager(this);
            Inventory = new Inventory();
            PlayerGuild = new Guild();

            QuestManager.Changed += Changed;
            Inventory.Changed += Changed;
            PlayerGuild.Changed += Changed;
        }
    }
}