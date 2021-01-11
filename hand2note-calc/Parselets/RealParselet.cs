using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  public class RealParselet : IPrefixParselet
  {
    public RealParselet()
    {
    }

    public Expression Parse(PrattParser parser, Token token)
    {
      return new RealExpression(token.Text, token.Location);
    }
  }
}
