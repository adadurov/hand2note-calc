using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  public interface IPrefixParselet
  {
    Expression Parse(PrattParser parser, Token token);
  }
}
