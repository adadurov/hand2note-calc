using NUnit.Framework;

using Hand2Note.Calc.Exceptions;
using Hand2Note.Calc.Expressions;

namespace Hand2Note.Calc.Test
{
  [TestFixture]
  public class ParserTest
  {
    [Test]
    public void Empty_Expression_Should_Trigger_Syntax_Error()
    {
      var text = "";
      var parser = new ExpressionParser(text);
      Assert.That(
        () => parser.Parse(),
        Throws.InstanceOf(typeof(SyntaxErrorException))
        );
    }

    [TestCase("3+")]
    [TestCase("3-")]
    [TestCase("2*")]
    [TestCase("*2")]
    [TestCase("3/")]
    [TestCase("/3")]
    [TestCase("abs")]
    [TestCase("abs +")]
    [TestCase("abs -")]
    [TestCase("abs *")]
    [TestCase("abs /")]
    public void Should_Yield_Syntax_Error(string expr)
    {
      var parser = new ExpressionParser(expr);
      Assert.That(
        () => parser.Parse(),
        Throws.InstanceOf(typeof(SyntaxErrorException))
        );
    }

    [Test]
    public void Should_Parse_Product_Expression()
    {
      var text = "2 * 2";
      var parser = new ExpressionParser(text);

      Assert.That(parser.Parse(),
        Is.InstanceOf<BinaryOpExpression>()
        .And.Property("OperationType").EqualTo(TokenType.MUL)
        .And.Property("Left").InstanceOf<RealExpression>()
        .And.Property("Left").Property("Value").EqualTo(2.0d)
        .And.Property("Right").InstanceOf<RealExpression>()
        .And.Property("Right").Property("Value").EqualTo(2.0d)
        );
    }

    [TestCase("abs (2)", ExpectedResult = 2d)]
    [TestCase("abs (-2)", ExpectedResult = 2d)]
    public double Should_Parse_Abs_Expression(string text)
    {
      var parser = new ExpressionParser(text);

      var expr = parser.Parse();

      Assert.That(expr, Is.InstanceOf<AbsExpression>());

      return expr.Calculate();
    }

    [Test]
    public void Too_Long_Real_Number_Should_Succeed()
    {
      // Given

      var text = "3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679";

      // When
      var parser = new ExpressionParser(text);

      var expr = parser.Parse() as RealExpression;

      // Then
      Assert.That(expr, Is.InstanceOf<RealExpression>());

      Assert.That(expr.Value, Is.EqualTo(3.1415926535897931d));
    }

  }
}
