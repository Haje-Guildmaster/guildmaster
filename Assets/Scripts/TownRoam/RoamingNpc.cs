using GuildMaster.Npcs;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.TownRoam
{
    public class RoamingNpc: GenericButton<NpcStaticData>
    {
        [SerializeField] private NpcStaticData npcData;
        protected override NpcStaticData EventArgument => npcData;

        public RoamingNpc(){}

        public RoamingNpc(NpcStaticData npcData)
        {
            this.npcData = npcData;
        }
    }
}