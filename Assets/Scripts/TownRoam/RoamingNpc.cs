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
        private void OnMouseUpAsButton()
        {
            clicked.Invoke(npcData);
        }
        
    }
}