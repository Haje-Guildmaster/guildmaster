
using System;
using System.Collections.Generic;
using GuildMaster.Items;

namespace GuildMaster.Exploration.Events
{
    public abstract class Instruction
    {
        public class PreModifiableInstruction<TSelf> : Instruction
        {
            public abstract class Modifier
            {
                public Condition Condition;
                public abstract void Modify(TSelf instruction);
            }

            public List<Modifier> Modifiers;
        }

        public class PerChance : Instruction
        {
            public float Chance;
            public Instruction Success;
            public Instruction Failure;
        }
        public class GetItem: Instruction
        {
            public Item Item;
            public int Number;
        }

        public class ChangeEnergy: PreModifiableInstruction<ChangeEnergy>
        {
            public enum EnergyType
            {
                Hp, Stamina
            }

            public EnergyType TargetType;
            public int Amount;
        }
        public class EndEvent : Instruction
        {}
    }
}