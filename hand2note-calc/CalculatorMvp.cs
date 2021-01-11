using Hand2Note.Calc.Exceptions;

namespace Hand2Note.Calc
{
    public class CalculatorMvp
    {
        public const int MaxExpressionlength = 2048;


        public double Calculate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return double.NaN;
            }

            if (input.Length > MaxExpressionlength)
            {
                throw new SyntaxErrorException(0, $"The expression is too long! Maximum supported length is {MaxExpressionlength}.");
            }

            try
            {
                return new ExpressionParser(input).Parse().Calculate();
            }
            catch (InvalidTokenException ex)
            {
                throw new SyntaxErrorException(ex);
            }
        }

    }
}
