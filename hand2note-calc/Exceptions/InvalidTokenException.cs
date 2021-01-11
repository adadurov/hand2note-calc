using System;

namespace Hand2Note.Calc.Exceptions
{
  public class InvalidTokenException : Exception
  {
    public InvalidTokenException(int location, string context) 
     : base($"Unrecognized token found at offset {location}. Context: '{context}'")
    {
      Location = location;
    }

    public int Location { get; }
  }
}
