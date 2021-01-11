using System.Collections.Generic;
using Hand2Note.Calc.Parselets;
using Hand2Note.Calc.Expressions;
using Hand2Note.Calc.Exceptions;
using System;

namespace Hand2Note.Calc
{
  public delegate void NewExpressionDelegate(Expression left);

  /// <summary>
  /// this class implements a Pratt parser
  /// it is an abstact class since it contains no grammar
  /// the grammar is provided via a constructor parameter
  /// </summary>
  /// <seealso cref="https://journal.stuffwithstuff.com/2011/03/19/pratt-parsers-expression-parsing-made-easy/"/>
  public class PrattParser
  {
    public IEnumerator<Token> TokenStream => Lexer;

    public IEnumerator<Token> Lexer { get; set; }

    List<Token> _lookAheadBuffer = new List<Token>();

    Dictionary<TokenType, IPrefixParselet> _prefixParselets;
    Dictionary<TokenType, IInfixParselet> _infixParselets;

    public NewExpressionDelegate NewExpressionFound;

    public PrattParser(IEnumerator<Token> lexer, Action<PrattParser> configurationAction)
    {
      Lexer = lexer;
      _prefixParselets = new Dictionary<TokenType, IPrefixParselet>();
      _infixParselets = new Dictionary<TokenType, IInfixParselet>();

      configurationAction?.Invoke(this);
    }

    public Expression ParseExpression()
    {
      return ParseExpression(0);
    }

    private void OnNewExpression(Expression left)
    {
      NewExpressionFound?.Invoke(left);
    }

    /// <summary>
    /// to be invoked from parselets!
    /// </summary>
    /// <param name="precedence"></param>
    /// <returns></returns>
    public Expression ParseExpression(int precedence)
    {
      var token = ConsumeNext();
      var prefixParselet = GetPrefixParselet(token.TokenType);

      if (null == prefixParselet)
      {
        throw new SyntaxErrorException(token.Location, $"Unexpected token: {token.Text}");
      }

      var left = prefixParselet.Parse(this, token);
      OnNewExpression(left);

      while (precedence < GetNextOperatorPrecedence())
      {
        token = ConsumeNext();

        var infixParselet = GetInfixParselet(token.TokenType);
        left = infixParselet.Parse(this, left, token);
        OnNewExpression(left);
      }
      return left;
    }

    public void AddPrefix(TokenType tokenType, IPrefixParselet parselet)
    {
      _prefixParselets.Add(tokenType, parselet);
    }

    public void AddInfix(TokenType tokenType, IInfixParselet parselet)
    {
      _infixParselets.Add(tokenType, parselet);
    }

    IPrefixParselet GetPrefixParselet(TokenType tokenType)
    {
      return _prefixParselets.ContainsKey(tokenType) ?
        _prefixParselets[tokenType] : null;
    }

    IInfixParselet GetInfixParselet(TokenType tokenType)
    {
      return _infixParselets.ContainsKey(tokenType) ?
        _infixParselets[tokenType] : null;
    }

    Token ConsumeNext()
    {
      LookAhead(0);
      var t = _lookAheadBuffer[0];
      _lookAheadBuffer.RemoveAt(0);
      return t;
    }

    public Token ConsumeNext(TokenType expectedType)
    {
      var t = LookAhead(0);
      if( t.TokenType != expectedType )
      {
        throw new SyntaxErrorException(
          t.Location, $"Unexpected token: {t.TokenType}. Expected: {expectedType}");
      }
      return ConsumeNext();
    }

    Token LookAhead(int distance)
    {
      while( _lookAheadBuffer.Count <= distance )
      {
        Lexer.MoveNext();
        _lookAheadBuffer.Add(Lexer.Current);
      }
      return _lookAheadBuffer[distance];
    }

    private int GetNextOperatorPrecedence()
    {
      var token = LookAhead(0);
      var parselet = GetInfixParselet(token.TokenType);
      if (parselet != null)
      {
        return parselet.GetPrecedence();
      }
      return 0;
    }

  }
}
