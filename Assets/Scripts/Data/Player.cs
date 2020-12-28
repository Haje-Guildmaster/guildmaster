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
        public readonly PlayerInventory PlayerInventory;
        public readonly InGameEventManager InGameEventManager;
        public readonly Guild PlayerGuild;

        private readonly Dictionary<NpcCode, Npc> _npcDataMap = new Dictionary<NpcCode, Npc>(); // Todo: NpcCode 이중저장. Set이나 리스트 사용 고려.

        public readonly Timemanagement TimeManager;



        /// <summary>
        /// Deprecated. Use GetNpc instead.
        /// </summary>
        /// <param name="npcCode"></param>
        /// <returns></returns>
        [Obsolete]
        [NotNull]
        public NpcStatus GetNpcStatus(NpcCode npcCode) => GetNpc(npcCode).Status;

        public Npc GetNpc(NpcCode npcCode)
        {
            return _npcDataMap.TryGetValue(npcCode, out var ret) ? ret : _AddNewNpc(npcCode);
        }
        
        public void ApplyReward(Reward reward)
        {
            switch (reward)
            {
                case Reward.AffinityReward affinityReward:
                    GetNpc(affinityReward.targetNpc).Status.Affinity.Value += affinityReward.amount;
                    break;
                case Reward.ItemReward itemReward:
                    PlayerInventory.TryAddItem(itemReward.item, itemReward.number);
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
        private Npc _AddNewNpc(NpcCode npcCode)
        {
            var made = new Npc(npcCode);
            _npcDataMap.Add(npcCode, made);
            made.Status.Changed += ()=>Changed?.Invoke();
            Changed?.Invoke();
            return made;
        }

        private static Player _instance;

        private Player()
        {
            QuestManager = new QuestManager(this);
            InGameEventManager = new InGameEventManager(this);
            PlayerInventory = new PlayerInventory(4, 36, false);
            PlayerGuild = new Guild();
            TimeManager = new Timemanagement();

            
            void InvokeChanged() => Changed?.Invoke();
            QuestManager.Changed += InvokeChanged;
            PlayerInventory.Changed += InvokeChanged;
            PlayerGuild.Changed += InvokeChanged;
        }
    }
}