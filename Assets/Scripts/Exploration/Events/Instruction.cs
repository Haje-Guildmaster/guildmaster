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

    [Serializable]
    public abstract class Instruction
    {
        [Serializable]
        public class PerChance : Instruction
        {
            [SerializeReference] [SerializeReferenceButton]
            public Expression Chance;

            public Sequential Success;
            public Sequential Failure;
        }

        [Serializable]
        public class GetItem : Instruction
        {
            public Item Item;

            [SerializeReference] [SerializeReferenceButton]
            public Expression Number;
        }

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

        [Serializable]
        public class EndEvent : Instruction
        {
        }

        [Serializable]
        public class Sequential : Instruction
        {
            [field: SerializeField] public FlavorText FlavorText { get; private set; }

            [SerializeReference] [SerializeReferenceButton]
            public List<Instruction> Instructions;
        }

        [Serializable]
        public class DoNothing : Instruction
        {
        }

        [Serializable]
        public class AddChoice : Instruction
        {
            public Event.Choice Choice;
        }

        [Serializable]
        public class If : Instruction
        {
            [SerializeReference] [SerializeReferenceButton]
            public Condition Condition;

            public Sequential IfTrue;
            public Sequential Else;
        }
    }
}