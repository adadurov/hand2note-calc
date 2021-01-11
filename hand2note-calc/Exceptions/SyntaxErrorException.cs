using System;

namespace Hand2Note.Calc.Exceptions
{
  public class SyntaxErrorException : Exception
  {
    public SyntaxErrorException(InvalidTokenException inner)
      : base(inner.Message, inner)
    {
      Location = inner.Location;
    }

    public SyntaxErrorException(int location, string message)
      : base(message)
    {
      Location = location;
    }

    public int Location { get; }
  }
}
