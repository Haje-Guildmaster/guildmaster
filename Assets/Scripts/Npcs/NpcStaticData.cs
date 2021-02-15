using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GuildMaster.Npcs
{
    [Serializable]
    public class NpcStaticData
    {
        public NpcBasicData basicData;
        public NpcRoamData roamData;
        public NpcQuestData questData;
        public NpcDealData dealData;
        public bool HasQuests => questData.HasQuests;
        public bool HasDeal => dealData.HasDeal;
    }
}