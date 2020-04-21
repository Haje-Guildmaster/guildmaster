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
        public bool HasQuests => questData.HasQuests;
    }
}