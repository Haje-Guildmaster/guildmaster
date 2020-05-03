using System.Collections.ObjectModel;
using System.Linq;
using GuildMaster.Database;
using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    public class ReadOnlyQuest
    {
        public ReadOnlyQuest(Quest quest)
        {
            _quest = quest;
        }

        public int QuestId => _quest.QuestId;
        public QuestCode QuestCode => _quest.QuestCode;
        public NpcCode Client => _quest.Client;
        public int StepIndex => _quest.StepIndex;
        public QuestStep CurrentStep => _quest.CurrentStep;
        public bool CanCompleteQuest => _quest.CanCompleteQuest;
        public bool CanCompleteStep => _quest.CanCompleteStep;
        public (StepMission mission, int progress)[] DoingMissions => _quest.DoingMissions;
        private readonly Quest _quest;
    }
}