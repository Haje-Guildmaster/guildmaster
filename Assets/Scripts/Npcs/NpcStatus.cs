using System;
using GuildMaster.Tools;

namespace GuildMaster.Npcs
{
    public class NpcStatus
    {
        public event Action Changed;

        public NpcStatus()
        {
            Affinity.Changed += ()=>Changed?.Invoke();
        }
        
        public readonly ChangeTrackedValue<int> Affinity = new ChangeTrackedValue<int>(0);
    }
}