using System.Collections.Generic;
using GuildMaster.Npcs.NpcInteractions;
using UnityEngine;

namespace GuildMaster.Npcs
{
    public class NpcData : ScriptableObject
    {
        public NpcBasicData basicData;
        public NpcRoamData roamData;
        public List<NpcInteraction> interactions;
    }
}