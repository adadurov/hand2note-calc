using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  /// <summary>
  /// Processes expressions like a+b, a-b, x^k, x=y
  /// </summary>
  public class BinaryOperatorParselet : IInfixParselet
  {
    int _precedence;
    bool _isRight;

    public BinaryOperatorParselet(bool isRightAssociative, int precedence)
    {
      _isRight = isRightAssociative;
      _precedence = precedence;
    }

    public Expression Parse(PrattParser parser, Expression left, Token token)
    {
      var right = parser.ParseExpression(
        _precedence - (_isRight ? 1 : 0));
      return new BinaryOpExpression(left, token, right);
    }

    public int GetPrecedence()
    {
      return _precedence;
    }
  }
}
