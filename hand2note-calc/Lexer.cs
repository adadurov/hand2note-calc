using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Hand2Note.Calc.Exceptions;

namespace Hand2Note.Calc
{
  /// <summary>
  /// Breaks the input expression text into known tokens
  /// forming a stream of tokens that is then consumed by parser.
  /// </summary>
  public class Lexer : IEnumerator<Token>
  {
    private readonly string _text;
    private readonly Regex _realMatcher;
    private readonly Dictionary<char, TokenType> _singleCharTokens;

    private int _index;
    private Token _currentToken;

    public Lexer(string text)
    {
      _text = text;
      _index = 0;
      _realMatcher = new Regex(@"[0-9]+(\.[0-9]+)?", RegexOptions.ExplicitCapture);

      _singleCharTokens = new Dictionary<char, TokenType>() {
        { '+', TokenType.PLUS },
        { '-', TokenType.MINUS },
        { '*', TokenType.MUL },
        { '/', TokenType.DIV },
        { '(', TokenType.LPAREN },
        { ')', TokenType.RPAREN }
      };
    }

    public Token Current => GetCurrent();

    object IEnumerator.Current => GetCurrent();

    private Token GetCurrent()
    {
      if (_currentToken == null)
      {
        throw new InvalidOperationException("Call MoveNext() before using the Current property");
      }
      return _currentToken ?? new Token(_currentToken.TokenType, _currentToken.Text, _index);
    }

    private Token Consume()
    {
      SkipCurrentToken();

      SkipWhitespace();

      // are we done with the input?
      if (EOF())
      {
        return CreateEOFToken();
      }

      if (MatchSingleCharToken(out Token token)) return token;

      // FIXME: Some day we'll need to support many functions.
      // Refactor this code before adding more supported tokens
      if (_text.HasSubstringAt(_index, "abs"))
      {
        return new Token(TokenType.ABS, "abs", _index);
      }

      if (MatchRealNumber(_text, _index, out Token real))
      {
        return real;
      }

      throw new InvalidTokenException(
        _index,
        _text.Substring(_index, Math.Min(8, 8 - (_index + 8 - _text.Length)))
        );
    }

    private bool MatchSingleCharToken(out Token token)
    {
      var nextChar = _text[_index];

      if (_singleCharTokens.TryGetValue(nextChar, out TokenType type))
      {
        token = new Token(type, nextChar.ToString(), _index);
        return true;
      }

      token = null;
      return false;
    }

    private bool EOF()
    {
      return _index >= _text.Length;
    }

    private Token CreateEOFToken()
    {
      return new Token(TokenType.EOF, "", _index);
    }

    private void SkipWhitespace()
    {
      // skip any whitespace following the current token
      for (; _index < _text.Length && Char.IsWhiteSpace(_text[_index]); ++_index) ;
    }

    private void SkipCurrentToken()
    {
      // skip current token
      if (null != _currentToken)
      {
        _index += _currentToken.Text.Length;
      }
    }

    private bool MatchRealNumber(string _text, int _index, out Token real)
    {
      var mc = _realMatcher.Matches(_text, _index);
      foreach (Match m in mc)
      {
        if (m.Groups[0].Success && m.Groups[0].Index == _index)
        {
          real = new Token(TokenType.REAL, m.Value, _index);
          return true;
        }
      }
      real = null;
      return false;
    }

    public bool MoveNext()
    {
      _currentToken = Consume();
      return true;
    }

    public void Reset()
    {
      _index = 0;
      _currentToken = null;
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~Lexer() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion
  }
}
