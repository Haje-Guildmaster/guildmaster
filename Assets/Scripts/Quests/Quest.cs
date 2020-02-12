using System;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    // 퀘스트 진행상황을 저장함.
    // QuestData의 iterator.
    public class Quest
    {
        
        public Quest(QuestData questData, NpcData client)
        {
            this.QuestData = questData;
            this.Client = client;
        }
        
        public readonly QuestData QuestData;
        public readonly NpcData Client;
        public int Index { get; private set; } = 0;
        public int StepProgress = 0;
        
        public QuestStep CurrentStep => QuestData.Steps.ElementAtOrDefault(Index);
        public bool CanCompleteQuest => CurrentStep == null;
        public bool CanCompleteStep => StepProgress >= CurrentStep.StepMission.MaxProgress;

        public void NextStep()
        {
            Index++;
            StepProgress = 0;
        }
    }
}