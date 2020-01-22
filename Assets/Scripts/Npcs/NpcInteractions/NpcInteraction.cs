using System;

namespace GuildMaster.Npcs.NpcInteractions
{
    [Serializable]
    public abstract class NpcInteraction
    {
        public abstract void Interact(NpcInteractUI ui, Action callback);
    }
}