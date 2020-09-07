using System;
using GuildMaster.Tools;

namespace GuildMaster.Npcs
{
    public class NpcStatus
    {
        // Todo: Character와 일관성 있게 수정.
        
        public event Action Changed;

        public NpcStatus()
        {
            Affinity.Changed += ()=>Changed?.Invoke();
        }
        
        public readonly ChangeTrackedValue<int> Affinity = new ChangeTrackedValue<int>(0);
    }
}