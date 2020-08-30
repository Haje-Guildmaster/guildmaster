using System;
using GuildMaster.Characters;

namespace GuildMaster.Exploration.Events
{
    public class ExpressionProcessor
    {
        public static int Calculate(Expression expression, Character selectedCharacter)
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
                            return selectedCharacter.Hp;
                        case Expression.CharacterStatus.Target.Stamina:
                            return selectedCharacter.Stamina;
                        case Expression.CharacterStatus.Target.Atk:
                            return selectedCharacter.Atk;
                        case Expression.CharacterStatus.Target.Agi:
                            return selectedCharacter.Agi;
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}