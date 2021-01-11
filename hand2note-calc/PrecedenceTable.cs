namespace Hand2Note.Calc
{
  public static class PrecedenceTable
  {
    public static readonly int FUNCTION = 1;
    public static readonly int SUM = 2;
    public static readonly int PROD = 3;
    public static readonly int POW = 4;
    public static readonly int PREFIX = 5;
  }
}
