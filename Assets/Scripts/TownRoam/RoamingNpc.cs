using GuildMaster.Npcs;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GuildMaster.TownRoam
{
    public class RoamingNpc: GenericButton<NpcCode>
    {
        [SerializeField] private NpcCode npc;
        protected override NpcCode EventArgument => npc;

        public RoamingNpc(){}

        public RoamingNpc(NpcCode npc)
        {
            this.npc = npc;
        }
    }
}