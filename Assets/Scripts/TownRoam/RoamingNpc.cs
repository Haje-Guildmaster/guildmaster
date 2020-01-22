using GuildMaster.Npcs;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.TownRoam
{
    public class RoamingNpc: GenericButton<NpcData>
    {
        [SerializeField] private NpcData npcData;
        protected override NpcData EventParameter => npcData;

        public RoamingNpc(){}

        public RoamingNpc(NpcData npcData)
        {
            this.npcData = npcData;
        }
    }
}