using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 탐색에서 하나의 (수학적) 식을 나타냅니다.
    /// "민첩*5 + 30" 같은 거.
    /// </summary>
    [Serializable]
    public abstract class Expression
    {
        private Expression()
        {
        }

        public enum Operator
        {
            Add, Multiply,
        }
        
        [Serializable]
        public class Constant : Expression
        {
            public float Value;
        }

        [Serializable]
        public class BinaryOperator : Expression
        {
            public enum OperatorType
            {
                Add, Subtract, Multiply
            }

            public OperatorType Type;
            [SerializeReference] [SerializeReferenceButton]
            public Expression Left;

            [SerializeReference] [SerializeReferenceButton]
            public Expression Right;
        }
        public class MultipleAdd : Expression
        {

            [SerializeReference]
            [SerializeReferenceButton]
            public List<Expression> expressions;
        }

        [Serializable]
        public class CharacterStatus : Expression
        {
            public enum Target
            {
                Hp,
                Stamina,
                Strength,
                Trick,
                Wisdom,
            }

            public Target StatusTarget;
        }
    }
}