using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GuildMaster.Npcs
{
    public class NpcData : ScriptableObject
    {
        public NpcBasicData basicData;
        public NpcRoamData roamData;
        public NpcQuestData questData;
        public bool HasQuests => questData.hasQuests;
    }
}