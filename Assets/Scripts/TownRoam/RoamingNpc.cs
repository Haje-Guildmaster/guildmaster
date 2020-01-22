using GuildMaster.Npcs;
using UnityEngine;
using UnityEngine.Events;

namespace GuildMaster.TownRoam
{
    [RequireComponent(typeof(Collider2D))]
    public class RoamingNpc: MonoBehaviour
    {
        [SerializeField] private NpcData npcData;
        public UnityEvent<NpcData> clicked;

        public RoamingNpc(){}

        public RoamingNpc(NpcData npcData)
        {
            this.npcData = npcData;
        }
        private void OnMouseUpAsButton()
        {
            clicked.Invoke(npcData);
        }
        
    }
}