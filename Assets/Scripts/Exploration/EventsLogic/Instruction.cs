using System;
using System.Collections.Generic;
using System.Security.Permissions;
using GuildMaster.Items;
using UnityEngine;

namespace GuildMaster.Exploration.Events
{
    [Serializable]
    public class FlavorText
    {
        public bool Exist => Text != "";
        [TextArea] public string Text;
    }

    /// <summary>
    /// 탐색에서 선택 후 일어나는 일에 대한 지시사항을 나타내는 클래스.
    /// </summary>
    [Serializable]
    public abstract class Instruction
    {
        /// <summary>
        /// 확률 체크를 하고 성공 여부에 따라 다음 지시를 수행함.
        /// </summary>
        [Serializable]
        public class PerChance : Instruction
        {
            [SerializeReference] [SerializeReferenceButton]
            public Expression Chance;

            public Sequential Success;
            public Sequential Failure;
        }

        /// <summary>
        /// 아이템을 얻음. Todo: GiveItem이 더 적합한 이름일 것 같음.
        /// </summary>
        [Serializable]
        public class GetItem : Instruction
        {
            public Item Item;

            [SerializeReference] [SerializeReferenceButton]
            public Expression Number;
        }

        /// <summary>
        /// 캐릭터의 hp/스태미나를 조작함.
        /// </summary>
        [Serializable]
        public class ChangeEnergy : Instruction
        {
            public enum EnergyType
            {
                Hp,
                Stamina
            }

            public EnergyType TargetType;

            [SerializeReference] [SerializeReferenceButton]
            public Expression Amount;
        }
        
        /// <summary>
        /// 이벤트를 종료시킴.
        /// </summary>
        [Serializable]
        public class EndEvent : Instruction
        {
        }

        /// <summary>
        /// 일련의 지시를 순서대로 수행함.
        /// </summary>
        [Serializable]
        public class Sequential : Instruction
        {
            [field: SerializeField] public FlavorText FlavorText { get; private set; }

            [SerializeReference] [SerializeReferenceButton]
            public List<Instruction> Instructions;
        }
        
        /// <summary>
        /// 아무것도 하지 않음. (현재 null 또한 같은 효과)
        /// </summary>
        [Serializable]
        public class DoNothing : Instruction
        {
        }
        
        /// <summary>
        /// 선택지를 추가함.
        /// </summary>
        [Serializable]
        public class AddChoice : Instruction
        {
            public Event.Choice Choice;
        }

        /// <summary>
        /// If 문.
        /// </summary>
        [Serializable]
        public class If : Instruction
        {
            [SerializeReference] [SerializeReferenceButton]
            public Condition Condition;

            public Sequential IfTrue;
            public Sequential Else;
        }

        /// <summary>
        /// 현재 선택지를 삭제함
        /// </summary>
        [Serializable]
        public class RemoveCurrentChoice : Instruction {}
    }
}