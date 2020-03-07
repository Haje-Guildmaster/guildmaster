using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [CreateAssetMenu(fileName = "Npc", menuName = "ScriptableObjects/NpcStaticData", order = 0)]
    public class NpcStaticData : ScriptableObject
    {
        public NpcBasicData basicData;
        public NpcRoamData roamData;
        public NpcQuestData questData;
        public bool HasQuests => questData.HasQuests;
    }
}