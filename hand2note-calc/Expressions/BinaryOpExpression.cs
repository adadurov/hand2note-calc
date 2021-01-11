using System;
using System.Text;

namespace Hand2Note.Calc.Expressions
{
  public class BinaryOpExpression : Expression
  {
    private Token _token;

    public Expression Left { get; }

    public Expression Right { get; }

    public BinaryOpExpression(Expression left, Token operation, Expression right)
      : base (operation.Location)
    {
      _token = operation;
      Left = left;
      Right = right;
    }

    public override double Calculate()
    {
      switch (_token.TokenType)
      {
        case TokenType.PLUS:
          return Left.Calculate() + Right.Calculate();

        case TokenType.MINUS:
          return Left.Calculate() - Right.Calculate();

        case TokenType.MUL:
          return Left.Calculate() * Right.Calculate();

        case TokenType.DIV:
          return Left.Calculate() / Right.Calculate();

        default:
          throw new NotImplementedException("Calculation not implemented for " + _token);
      }
    }

    public TokenType OperationType
    {
      get
      {
        return _token.TokenType;
      }
    }

  }
}
