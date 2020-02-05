using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    public struct ReadonlyQuest
    {
        private readonly Quest _quest;
        public ReadonlyQuest(Quest quest)
        {
            _quest = quest;
        }

        public NpcData Client => _quest.Client;
        public int Index => _quest.Index;
        public QuestStep CurrentStep => _quest.CurrentStep;
        public QuestData QuestData => _quest.QuestData;
        public int StepProgress => _quest.StepProgress;
        public bool CanCompleteQuest => _quest.CanCompleteQuest;
        public bool CanCompleteStep => _quest.CanCompleteStep;
    }
}