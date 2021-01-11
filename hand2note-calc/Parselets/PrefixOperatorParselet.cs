using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  /// <summary>
  /// parses prefix '+' and '-' operators
  /// </summary>
  class PrefixOperatorParselet : IPrefixParselet
  {
    int _precedence;

    public PrefixOperatorParselet(int precedence)
    {
      _precedence = precedence;
    }

    public Expression Parse(PrattParser parser, Token token)
    {
      var operand = parser.ParseExpression(_precedence);
      return new PrefixExpression(token.TokenType, token.Location, operand);
    }
  }
}
