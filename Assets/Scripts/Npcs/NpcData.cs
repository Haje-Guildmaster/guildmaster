using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [CreateAssetMenu(fileName = "Npc", menuName = "ScriptableObjects/NpcData", order = 0)]
    public class NpcData : ScriptableObject
    {
        public int npcDataId;
        public NpcBasicData basicData;
        public NpcRoamData roamData;
        public NpcQuestData questData;
        public bool HasQuests => questData.HasQuests;
    }
}