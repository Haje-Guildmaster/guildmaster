using System;
using GuildMaster.Conditions;
using GuildMaster.Dialog;
using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    public abstract class StepMission
    {
        public abstract int MaxProgress { get; }
        
        private StepMission() {}

        [Serializable]
        public class TalkStep : StepMission
        {
            public override int MaxProgress => 1;
            public NpcData talkTo;
            public Script script;
        }
    }
}