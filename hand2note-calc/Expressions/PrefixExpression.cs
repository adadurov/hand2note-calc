using System;
using System.Text;

namespace Hand2Note.Calc.Expressions
{
  public class PrefixExpression : Expression
  {
    public Expression Operand { get; }

    private TokenType _operationType;

    public PrefixExpression(TokenType operationType, int location, Expression operand)
      : base(location)
    {
      _operationType = operationType;
      Operand = operand;
    }

    public override double Calculate()
    {
      switch (_operationType)
      {
        case TokenType.PLUS:
          return Operand.Calculate();

        case TokenType.MINUS:
          return - Operand.Calculate();

        default:
          throw new NotImplementedException();
      }
    }
  }
}
