using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 이벤트 하나를 나타내는 데이터 클래스.
    /// </summary>
    [Serializable]
    public sealed class Event
    {
        [Serializable]
        public class Choice
        {
            [TextArea] public string Description;
            public bool OneOff = false;            // 1번 사용하고 사라지는 선택인지. 선택지가 이벤트를 끝내다면 의미없음.

            [FormerlySerializedAs("Instruction")] public Instruction.Sequential Sequential;
        }

        public string ShortDescription;
        [TextArea] public string FullDescription;
        public List<Choice> Choices;
    }
}