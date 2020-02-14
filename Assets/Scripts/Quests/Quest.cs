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

        public class MissionProgress
        {
            public StepMission Mission;
            public int Progress;
            public bool Completed => Progress >= Mission.MaxProgress;
        }
        public readonly int QuestId;
        public readonly QuestData QuestData;
        public readonly NpcData Client;

        public int StepIndex { get; private set; }
        public QuestStep CurrentStep => QuestData.Steps.ElementAtOrDefault(StepIndex);
        public bool CanCompleteQuest => CurrentStep == null;
        public bool CanCompleteStep => DoingMissions.Aggregate(true, 
            (prev,m)=> prev && (m.Progress >= m.Mission.MaxProgress));
        public void NextStep()
        {
            StepIndex++;
            DoingMissions = CurrentStep?.StepMissions
                .Select(m => new MissionProgress{Mission = m}).ToArray();
        }
        public MissionProgress[] DoingMissions { get; private set; }

        private static int _idCnt = 0;
    }
}