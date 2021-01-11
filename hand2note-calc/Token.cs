namespace Hand2Note.Calc
{
  public class Token
  {
    /// <summary>
    /// The constant used to indicate that the token has no specific location
    /// </summary>
    public const int NoLocation = -1;

    public TokenType TokenType { get; }

    public string Text { get; }

    public int Location { get; }

    public override string ToString() => Text;

    public Token(TokenType type, string text, int location)
    {
      TokenType = type;
      Text = text;
      Location = location;
    }

  }
}
