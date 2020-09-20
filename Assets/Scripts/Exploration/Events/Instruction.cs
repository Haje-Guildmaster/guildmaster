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
        [field: SerializeField] public FlavorText FlavorText { get; private set; }

        [Serializable]
        public class PerChance : Instruction
        {
            [SerializeReference] [SerializeReferenceButton]
            public Expression Chance;

            [SerializeReference] [SerializeReferenceButton]
            public Instruction Success;

            [SerializeReference] [SerializeReferenceButton]
            public Instruction Failure;
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
            [SerializeReference] [SerializeReferenceButton]
            public List<Instruction> Instructions;
        }

        [Serializable]
        public class DoNothing : Instruction
        {}
    }
}