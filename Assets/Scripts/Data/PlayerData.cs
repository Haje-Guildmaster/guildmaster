using System;
using System.Collections.Generic;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.InGameEvents;
using GuildMaster.Items;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.Rewards;
using GuildMaster.Tools;
using JetBrains.Annotations;
using UnityEngine;

namespace GuildMaster.Data
{
    // 게임을 플레이하는 사람의 모든 정보, 즉 세이브를 했을 때 저장되는 모든 데이터를 담습니다.
    // 퀘스트 클리어 정보, 길드원들, 레벨, 장비, etc...

    public class PlayerData
    {
        public static PlayerData Instance
        {
            get { return _instance = _instance ?? new PlayerData(); }
        }

        public event Action Changed;
        public event Action InventoryChanged;
        public readonly QuestManager QuestManager;
        public readonly InGameEventManager InGameEventManager;
        private readonly Dictionary<NpcStaticData, NpcStatus> _npcStatusMap = new Dictionary<NpcStaticData, NpcStatus>();
        private readonly Dictionary<Item, int> _inventoryMap = new Dictionary<Item, int>();

        public IEnumerable<(Item item, int number)> GetInventory() => _inventoryMap.Select(a=>(a.Key, a.Value));
        private int _level = 1;
        
        [NotNull] public NpcStatus GetNpcStatus(NpcStaticData npc)
        {
            return _npcStatusMap.TryGetValue(npc, out var ret) ? ret: _AddNpcStatus(npc);
        }

        public void ApplyReward(Reward reward)
        {
            switch (reward)
            {
                case Reward.AffinityReward affinityReward:
                    GetNpcStatus(affinityReward.targetNpc).Affinity += affinityReward.amount;
                    break;
                case Reward.ItemReward itemReward:
                    TryAddItem(itemReward.item, itemReward.number);
                    break;
                default:
                    throw new Exception($"Unexpected Reward: {reward}");
            }
        }

        public bool TryAddItem(Item item, int number)
        {
            _inventoryMap.TryGetValue(item, out var prevItemNum);
            var itemData = ItemDatabaseLoader.Loaded.GetElement(item.Code);
            var updatedNumber = prevItemNum + number;
            if (updatedNumber <= itemData.MaxStack)
                _inventoryMap[item] = updatedNumber;
            else
            {
                //미정.
                _inventoryMap[item] = itemData.MaxStack;
            }
            
            if (prevItemNum != _inventoryMap[item])
                InventoryChanged?.Invoke();

            return true;
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
                    return QuestManager.CompletedQuest(hasCompletedQuest.quest);
                case Condition.LevelOver levelOver:
                    return levelOver.level > _level;
                default:
                    throw new Exception($"Unexpected Condition: {condition}");
            }
        }


        [NotNull] private NpcStatus _AddNpcStatus(NpcStaticData npc)
        {
            var made = new NpcStatus();
            _npcStatusMap.Add(npc, made);
            made.Changed += Changed;
            Changed?.Invoke();
            return made;
        }

        private static PlayerData _instance;

        private PlayerData()
        {
            QuestManager = new QuestManager(this);
            InGameEventManager = new InGameEventManager(this);
            QuestManager.Changed += Changed;
            InventoryChanged += Changed;
        }
    }
}