using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Parselets
{
  public interface IInfixParselet
  {
    Expression Parse(PrattParser parser, Expression left, Token token);

    int GetPrecedence();
  }
}
