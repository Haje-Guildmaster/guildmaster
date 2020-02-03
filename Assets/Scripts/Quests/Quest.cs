using System;
using System.Linq;
using GuildMaster.Conditions;
using GuildMaster.Data;
using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    // 퀘스트 진행상황을 저장함.
    // QuestData의 iterator.
    public class Quest
    {
        public Quest(QuestData questData)
        {
            this.QuestData = questData;
        }
        
        public readonly QuestData QuestData;
        public int Index { get; private set; } = 0;
        public int StepProgress = 0;
        
        public QuestStep CurrentStep => QuestData.Steps.ElementAtOrDefault(Index);
        public bool CanCompleteQuest => CurrentStep == null;
        public bool CanCompleteStep => StepProgress >= CurrentStep.StepMission.MaxProgress;

        public void NextStep() => Index++;

    }
}