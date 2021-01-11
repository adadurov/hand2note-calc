using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hand2Note.Calc.Expressions
{
  public class RealExpression : Expression
  {
    public double Value { get; }

    public RealExpression(string text, int location)
      : base(location)
    {
      Value = double.Parse(text, CultureInfo.InvariantCulture);
    }

    public override double Calculate()
    {
      return Value;
    }
  }
}
