using GuildMaster.Dialog;
using GuildMaster.Npcs;
using GuildMaster.Quests;
using GuildMaster.InGameEvents;
using UnityEngine.Events;

namespace GuildMaster.Events
{
    // 게임에서 일어나는 여러 이벤트를 제공합니다.
    // 객체 간의 통신 중계 역할입니다.
    public static class GameEvents
    {
        private class QuestScriptPlayEndEvent : UnityEvent<StepMission.TalkMission> {}
        public static readonly UnityEvent<StepMission.TalkMission> QuestScriptPlayEnd = new QuestScriptPlayEndEvent();
        public static readonly UnityEvent QuestManagerDataChange = new UnityEvent();

        //private class InGameEventOccurEvent : UnityEvent<InGameEventData> {} 
        public static readonly UnityEvent InGameEventManagerEventOccur = new UnityEvent(); 
        //이벤트가 발생할수있는 상황에 대한 코드를 짠뒤 수정해야함 (상황에 대한 인자를 받거나 실행해야할 이벤트의 데이터를 받거나?)
    }
}