using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  public class GroupParselet : IPrefixParselet
  {
    public Expression Parse(PrattParser parser, Token token)
    {
      var expression = parser.ParseExpression();
      parser.ConsumeNext(TokenType.RPAREN);
      return expression;
    }
  }
}
