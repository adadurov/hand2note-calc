using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  public class AbsFunctionParselet : IPrefixParselet
  {
    public Expression Parse(PrattParser parser, Token token)
    {
      parser.ConsumeNext(TokenType.LPAREN);
      var operand = parser.ParseExpression(PrecedenceTable.FUNCTION);
      parser.ConsumeNext(TokenType.RPAREN);
      return new AbsExpression(token.Location, operand);
    }
  }
}
