using System;
using System.Linq;
using GuildMaster.Conditions;
using GuildMaster.Data;
using GuildMaster.Npcs;

namespace GuildMaster.Quests
{
    using Script = String;
    // 퀘스트 진행상황을 저장함.
    // QuestData의 iterator에 가까우나 QuestData안의 일부 정보를 저장하기에 QuestData가 수정되면 오류가 나기 쉽습니다.
    [Serializable]
    public class Quest
    {
        public readonly QuestData QuestData;
        private int _index;
        private Condition _stepCompleteCondition;
        private bool _completed = false;
        private NpcData _reportTo;
        private Script _reportScript;
        
        public Condition StepCompleteCondition => _stepCompleteCondition;
        public bool Completed => _completed;
        public NpcData ReportTo => _reportTo;
        public string ReportScript => _reportScript;

        public Quest(QuestData questData)
        {
            this.QuestData = questData;

            _index = -1;
            NextStep();
        }

        public bool TryCompleteStep(PlayerData playerData)
        {
            if (Completed) return false;
            if (!playerData.CheckCondition(_stepCompleteCondition))
                return false;
            NextStep();
            return true;
        }

        private void NextStep()
        {
            _index++;
            var nextStep = QuestData.Steps.ElementAtOrDefault(_index);
            if (nextStep == null)
                MarkCompleted();
            else
                LoadQuestStep(nextStep);
        }
        private void LoadQuestStep(QuestData.Step step)
        {
            _stepCompleteCondition = step.completeCondition.GenerateCompletionCondition(PlayerData.Instance);
            _reportScript = step.reportScript;
            _reportTo = step.reportTo;
        }

        private void MarkCompleted()
        {
            _completed = true;
        }
    }
}