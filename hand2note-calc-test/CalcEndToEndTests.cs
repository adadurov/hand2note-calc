using NUnit.Framework;

using Hand2Note.Calc.Exceptions;
using System.Text;

namespace Hand2Note.Calc.Test
{
  [TestFixture]
  public class CalcEndToEndTests
  {
    // invalid tokens (inner exception)
    [TestCase("фыва+0", 0)]
    [TestCase( "z", 0)]
    [TestCase(@"1 \ 1", 2)]
    [TestCase( "1 = 1", 2)]
    public void Negative_InvalidToken(string input, int location)
    {
      Assert.That(
        () => new CalculatorMvp().Calculate(input),
        Throws
            .InstanceOf<SyntaxErrorException>()
            .With.Property("Location").EqualTo(location)
            .With.Property("InnerException").InstanceOf<InvalidTokenException>()
        );
    }

    [TestCase( "* 2", 0)]
    [TestCase( "/ 2", 0)]
    [TestCase( "abs 2", 4)]
    [TestCase( "2+abs", 5)]

    public void Negative_SyntaxError(string input, int location)
    {
      Assert.That(
        () => new CalculatorMvp().Calculate(input),
        
        Throws
            .InstanceOf<SyntaxErrorException>()
            .With.Property("Location").EqualTo(location)
        );
    }

    [TestCase("()", 1)]
    [TestCase(")", 0)]
    [TestCase("(", 1)]
    [TestCase("())", 1)]
    [TestCase("1+2)", 3)]
    [TestCase("(1+2", 4)]
    public void Negative_Parentheses(string input, int location)
    {
       Negative_SyntaxError(input, location);
    }

    // as '0' in double is not really a zero,
    // these expressions should not throw Division By Zero Exception
    [TestCase("1/0")]
    [TestCase("(234+572)/(232-232)")]
    public void Division_By_Zero_Should_Not_Throw(string input)
    {
      new CalculatorMvp().Calculate(input);
    }

    [TestCase("", ExpectedResult = double.NaN)]
    [TestCase("-1.5", ExpectedResult = -1.5d)]
    [TestCase("+0", ExpectedResult = 0d)]
    [TestCase("1.0000000", ExpectedResult = 1d)]

    [TestCase("-11.0000000+0", ExpectedResult = -11d)]
    [TestCase("-11.0000000-0", ExpectedResult = -11d)]

    [TestCase("-11.0000000+12.000000000", ExpectedResult = 1d)]
    [TestCase("11.0000000-12.000000000", ExpectedResult = -1d)]

    [TestCase("11.0000000*12.000000000", ExpectedResult = 132d)]
    [TestCase("11.0000000*-12.000000000", ExpectedResult = -132d)]
    [TestCase("-11.0000000*12.000000000", ExpectedResult = -132d)]
    [TestCase("-11.0000000*-12.000000000", ExpectedResult = 132d)]

    [TestCase("12.0000000*0", ExpectedResult = 0d)]
    [TestCase("-12.0000000*0", ExpectedResult = 0d)]

    [TestCase("1024 / 256", ExpectedResult = 4d)]
    [TestCase("-1024 / 256", ExpectedResult = -4d)]
    [TestCase("1024 / -256", ExpectedResult = -4d)]
    [TestCase("-1024 / -256", ExpectedResult = 4d)]

    [TestCase("abs( -1024 / 256 / 1)", ExpectedResult = 4d)]
    [TestCase("abs( -1024 / 256 / -1)", ExpectedResult = 4d)]
    [TestCase("abs( 1024 - 1024)", ExpectedResult = 0d)]

    public double Positive_Simple(string input)
    {
      return new CalculatorMvp().Calculate(input);
    }

    [TestCase("1 * (9+1) * 2.02", ExpectedResult = 20.2d)]
    [TestCase("(1 * ((9+1))) * 2.02", ExpectedResult = 20.2d)]
    public double Positive_Parentheses(string input)
    {
      return new CalculatorMvp().Calculate(input);
    }

    [Test]
    public void Positive_Max_Expr_Length_Should_Succeed()
    {
      var N = CalculatorMvp.MaxExpressionlength / 2;
      
      var sb = new StringBuilder(CalculatorMvp.MaxExpressionlength);

      for (var i = 0; i < N; i++)
      {
        sb.Append("+1");
      }

      var expr = sb.ToString();

      Assert.That(expr.Length, Is.EqualTo(CalculatorMvp.MaxExpressionlength));

      Assert.That(
        new CalculatorMvp().Calculate(expr),
        Is.EqualTo( (double)N )
        );

    }
  }
}
