using System;
using System.Linq;
using GuildMaster.Database;

namespace GuildMaster.Quests
{
    // 퀘스트 진행상황을 저장함.
    // QuestData의 iterator.
    public class Quest
    {
        public Quest(QuestCode questCode, NpcCode client)
        {
            QuestId = _idCnt++;
            QuestCode = questCode;
            Client = client;
            StepIndex = -1;
            NextStep();
        }
        
        public readonly int QuestId;
        public readonly QuestCode QuestCode;
        public readonly NpcCode Client;

        public int StepIndex { get; private set; }
        public QuestStep CurrentStep => QuestDatabase.Get(QuestCode).Steps.ElementAtOrDefault(StepIndex);
        public bool CanCompleteQuest => CurrentStep == null;
        public bool CanCompleteStep => DoingMissions.Aggregate(true, 
            (prev,m)=> prev && (m.progress >= m.mission.MaxProgress));
        public void NextStep()
        {
            StepIndex++;
            _progresses = new int[CurrentStep?.StepMissions.Count ?? 0];
        }
        public (StepMission mission, int progress)[] DoingMissions =>
            CurrentStep.StepMissions.Select((m, i) => (m, _progresses[i])).ToArray();

        public void SetProgress(int index, int progress)
        {
            _progresses[index] = progress;
        }
        
        private int[] _progresses;
        private static int _idCnt = 0;
    }
}