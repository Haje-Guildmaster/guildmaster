using System;
using GuildMaster.Characters;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// Expression을 계산하는 역할입니다.
    /// </summary>
    public static class ExpressionProcessor
    {
        /// <summary>
        /// Expression을 계산합니다.
        /// </summary>
        /// <param name="expression"> 식 </param>
        /// <param name="selectedCharacter"> 대상 캐릭터 </param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"> Unexpected Expression </exception>
        public static float Calculate(Expression expression, Character selectedCharacter)
        {
            switch (expression)
            {
                case Expression.BinaryOperator binaryOperator:
                    var left = Calculate(binaryOperator.Left, selectedCharacter);
                    var right = Calculate(binaryOperator.Right, selectedCharacter);
                    switch (binaryOperator.Type)
                    {
                        case Expression.BinaryOperator.OperatorType.Add:
                            return left + right;
                        case Expression.BinaryOperator.OperatorType.Multiply:
                            return left * right;
                        default:
                            throw new NotImplementedException();
                    }
                case Expression.Constant constant:
                    return constant.Value;
                case Expression.CharacterStatus characterStatus:
                    switch (characterStatus.StatusTarget)
                    {
                        case Expression.CharacterStatus.Target.Hp:
                            return (float)selectedCharacter.Hp;
                        case Expression.CharacterStatus.Target.Stamina:
                            return (float)selectedCharacter.Stamina;
                        case Expression.CharacterStatus.Target.Strength:
                            return (float)selectedCharacter.Strength;
                        case Expression.CharacterStatus.Target.Trick:
                            return (float)selectedCharacter.Trick;
                        case Expression.CharacterStatus.Target.Wisdom:
                            return (float)selectedCharacter.Wisdom;
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}