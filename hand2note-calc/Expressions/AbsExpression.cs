using System;

namespace Hand2Note.Calc.Expressions
{
  public class AbsExpression : Expression
  {
    public Expression Operand { get; }

    public AbsExpression(int location, Expression operand)
      : base(location)
    {
      Operand = operand;
    }

    public override double Calculate()
    {
      return Math.Abs(Operand.Calculate());
    }

  }
}
