using System;
using System.Linq;
using GuildMaster.Data;
using GuildMaster.Npcs;
using UnityEditorInternal;

namespace GuildMaster.Quests
{
    // 퀘스트 진행상황을 저장함.
    // QuestData의 iterator.
    public class Quest
    {
        public Quest(QuestData questData, NpcData client)
        {
            QuestId = _idCnt++;
            QuestData = questData;
            Client = client;
            StepIndex = -1;
            NextStep();
        }
        
        public readonly int QuestId;
        public readonly QuestData QuestData;
        public readonly NpcData Client;

        public int StepIndex { get; private set; }
        public QuestStep CurrentStep => QuestData.Steps.ElementAtOrDefault(StepIndex);
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