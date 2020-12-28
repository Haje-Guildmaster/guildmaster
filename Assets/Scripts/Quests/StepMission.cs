using System;
using GuildMaster.Databases;
using GuildMaster.Dialogs;
using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    [Serializable]
    public abstract class StepMission
    {
        public abstract int MaxProgress { get; }
        
        private StepMission() {}

        [Serializable]
        public class TalkMission : StepMission
        {
            public override int MaxProgress => 1;
            public NpcCode talkTo;
            public Script script;
        }
    }
}