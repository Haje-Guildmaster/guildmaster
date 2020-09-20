using System;
using System.Linq.Expressions;
using UnityEngine;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 탐색에서 하나의 (수학적) 식을 나타냅니다.
    /// "민첩*5 + 30"의 데미지 같은 거.
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
                Add, Multiply
            }

            public OperatorType Type;
            [SerializeReference] [SerializeReferenceButton]
            public Expression Left;

            [SerializeReference] [SerializeReferenceButton]
            public Expression Right;
        }
        
        [Serializable]
        public class CharacterStatus : Expression
        {
            public enum Target
            {
                Hp,
                Stamina,
                Atk,
                Agi,
            }

            public Target StatusTarget;
        }
    }
}