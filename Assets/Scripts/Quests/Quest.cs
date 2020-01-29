using System;

namespace GuildMaster.Quests
{
    // 퀘스트 진행상황을 저장함.
    [Serializable]
    public class Quest
    {
        private QuestData _questData;
        private int _index = 0;
        
        
        public Quest(QuestData questData)
        {
            this._questData = questData;
        }
    }
}