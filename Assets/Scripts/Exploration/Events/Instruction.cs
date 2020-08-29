
using System;
using System.Collections.Generic;
using GuildMaster.Items;
using UnityEngine;

namespace GuildMaster.Exploration.Events
{
    [Serializable]
    public abstract class Instruction
    {
        [Serializable]
        public abstract class PreModifiableInstruction<TSelf> : Instruction
        {
            public abstract class Modifier
            {
                public Condition Condition;
                public abstract void Modify(TSelf instruction);
            }

            public List<Modifier> Modifiers;
        }

        [Serializable]
        public class PerChance : Instruction
        {
            public float Chance;
            [SerializeReference][SerializeReferenceButton] public Instruction Success;
            [SerializeReference][SerializeReferenceButton] public Instruction Failure;
        }
        [Serializable]
        public class GetItem: Instruction
        {
            public Item Item;
            public int Number;
        }

        [Serializable]
        public class ChangeEnergy: PreModifiableInstruction<ChangeEnergy>
        {
            public enum EnergyType
            {
                Hp, Stamina
            }

            public EnergyType TargetType;
            public int Amount;
        }
        [Serializable]
        public class EndEvent : Instruction
        {}
    }
}