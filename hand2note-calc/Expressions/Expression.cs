using System.Text;
using System.Collections.Generic;

namespace Hand2Note.Calc.Expressions
{
  public abstract class Expression
  {
    /// <summary>
    /// return location of the expression in the input text
    /// useful for reporting errors
    /// </summary>
    public int Location { get; }

    public Expression(int location)
    {
      Location = location;
    }

    public abstract double Calculate();
  }
}
