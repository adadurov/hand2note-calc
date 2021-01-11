namespace Hand2Note.Calc
{
  public enum TokenType
  {
    MINUS,

    PLUS,

    REAL,

    EOF,

    MUL, // fake token (as multiplication is omitted in our grammar)

    DIV,

    LPAREN,

    RPAREN,

    ABS
  }
}
