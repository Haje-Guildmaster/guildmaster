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

    }
}