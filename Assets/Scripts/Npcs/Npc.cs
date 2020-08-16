using GuildMaster.Databases;

namespace GuildMaster.Npcs
{
    public class Npc
    {
        public Npc(NpcCode code)
        {
            Code = code;
        }
        
        public readonly NpcStatus Status = new NpcStatus();
        public readonly NpcCode Code;
        public NpcStaticData StaticData => NpcDatabase.Get(Code);

        public static implicit operator NpcCode(Npc npc) => npc.Code;
    }
}