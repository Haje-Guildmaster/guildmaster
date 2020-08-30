using System;
using System.Collections.Generic;
using GuildMaster.Items;
using UnityEngine;

namespace GuildMaster.Exploration.Events
{
    [Serializable]
    public abstract class Instruction
    {
        // 일단 이거 보류.
        // 아마 후에 지울 듯.
        // [Serializable]
        // public class PreModifiableInstruction<TSelf> : Instruction
        // {
        //     [Serializable]
        //     public abstract class Modifier
        //     {
        //         public Condition Condition;
        //         public abstract void Modify(TSelf instruction);
        //     }
        //
        //     [SerializeReference][SerializeReferenceButton]public List<Modifier> Modifiers;
        // }

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
    }

}

