using NUnit.Framework;

using Hand2Note.Calc.Expressions;
using System;

namespace Hand2Note.Calc.Test
{
  [TestFixture]
  public class ExprConverterTest
  {
    [TestCase("1", ExpectedResult = typeof(RealExpression))]
    [TestCase("17.3 + 13", ExpectedResult = typeof(BinaryOpExpression))]
    
    public Type Number_Should_Produce_Real_Expression(string expressionText)
    {
      return new ExpressionParser(expressionText).Parse().GetType();
    }

    [Test]
    public void Sum_Should_Produce_Sum_Expression()
    {
      var parser = new ExpressionParser("17.3+13");

      // When
      var expr = parser.Parse();

      // Then
      Assert.That(expr, Is.InstanceOf<BinaryOpExpression>());

      var opEx = expr as BinaryOpExpression;
      Assert.That(opEx.OperationType, Is.EqualTo(TokenType.PLUS));
    }

    [Test]
    public void Abs_Should_Produce_Abs_Expression()
    {
      var parser = new ExpressionParser("abs(-13)");

      // When
      var expr = parser.Parse();

      // Then
      Assert.That(expr, Is.InstanceOf<AbsExpression>());

      var opEx = expr as AbsExpression;
      Assert.That(opEx.Operand.Calculate(), Is.EqualTo( -13d ));
    }

  }
}
