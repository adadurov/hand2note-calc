using Hand2Note.Calc.Expressions;
using Hand2Note.Calc.Parselets;
using Hand2Note.Calc.Exceptions;

namespace Hand2Note.Calc
{
  public class ExpressionParser
  {
    private readonly PrattParser _parser;

    /// <summary>
    /// Initializes a new instance of the parser with the expression 
    /// that should be parsed.
    /// </summary>
    /// <param name="text"></param>
    /// <remarks>
    /// Notice that the parser is a one-time-use object!
    /// </remarks>
    public ExpressionParser(string text) 
    {
      _parser = new PrattParser(new Lexer(text), _ => RegisterParselets(_));
    }

    /// <summary>
    /// Parses the expression and tests that the root expression is
    /// 'equality' expression. That is, an OperatorExpression with the TokenType.EQUALITY
    /// </summary>
    /// <returns></returns>
    /// <exception cref="">Throws SyntaxErrorException if the root operator is not as expected and on other syntax errors.</exception>
    public Expression Parse()
    {
      var expr = _parser.ParseExpression();
      
      // is the expression fully parsed?
      if( _parser.TokenStream.Current.TokenType != TokenType.EOF )
      {
        throw new SyntaxErrorException(
          _parser.TokenStream.Current.Location,
          "Unexpected token: " + _parser.TokenStream.Current.Text);
      }
      return expr;
    }

    /// <summary>
    /// Specfify the most part of the grammar for the parser
    /// The remaining part of the grammar is defined the static <see cref="ExpressionParser"/>
    /// constructor and limited by the <see cref="ExpressionParser.OnNewExpression"/> routine
    /// </summary>
    private void RegisterParselets(PrattParser parser)
    {
      parser.AddPrefix(TokenType.REAL, new RealParselet());

      parser.AddPrefix(TokenType.LPAREN, new GroupParselet());
      parser.AddPrefix(TokenType.PLUS, new PrefixOperatorParselet(PrecedenceTable.PREFIX));
      parser.AddPrefix(TokenType.MINUS, new PrefixOperatorParselet(PrecedenceTable.PREFIX));
      parser.AddPrefix(TokenType.ABS, new AbsFunctionParselet());

      parser.AddInfix(TokenType.PLUS, new BinaryOperatorParselet(false, PrecedenceTable.SUM));
      parser.AddInfix(TokenType.MINUS, new BinaryOperatorParselet(false, PrecedenceTable.SUM));
      parser.AddInfix(TokenType.MUL, new BinaryOperatorParselet(false, PrecedenceTable.PROD));
      parser.AddInfix(TokenType.DIV, new BinaryOperatorParselet(false, PrecedenceTable.PROD));
    }

    /// <summary>
    /// this routine enforces some limitations not expressly defined by the 'static' grammar
    /// </summary>
    /// <param name="expr"></param>
    protected void OnNewExpression(Expression expr)
    {
    }
  }
}
