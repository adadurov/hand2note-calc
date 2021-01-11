using System;
using System.Collections.Generic;
using NUnit.Framework;

using Hand2Note.Calc.Exceptions;

namespace Hand2Note.Calc.Test 
{
  [TestFixture]
  public class LexerTest
  {
    [Test]
    public void Current_Without_MoveNext_Should_Throw()
    {
      var lexer = new Lexer("");
      Assert.That(
        () => lexer.Current,
        Throws.InstanceOf(typeof(InvalidOperationException))
        );
    }

    [Test]
    public void Empty_String_Should_Produce_EOF()
    {
      var lexer = new Lexer(string.Empty);
      Assert.That(lexer.MoveNext(), Is.True);
      Assert.That(lexer.Current.TokenType, Is.EqualTo(TokenType.EOF));
    }

    [Test]
    public void Empty_String_Should_Produce_Multiple_EOFs_For_Lookahead()
    {
      var lexer = new Lexer(string.Empty);
      var EOF = new Token(TokenType.EOF, "", 0);
      Assert.That(lexer.MoveNext(), Is.True);
      ExpectToken(lexer, EOF);
      Assert.That(lexer.MoveNext(), Is.True);
      ExpectToken(lexer, EOF);
      Assert.That(lexer.MoveNext(), Is.True);
      ExpectToken(lexer, EOF);
    }

    [Test]
    public void WhitespaceOnly_Should_Produce_EOF()
    {
      var lexer = new Lexer(" \t");
      Assert.That(lexer.MoveNext(), Is.True);
      Assert.That(lexer.Current.TokenType, Is.EqualTo(TokenType.EOF));
    }

    [TestCase("+", TokenType.PLUS)]
    [TestCase("-", TokenType.MINUS)]
    [TestCase("*", TokenType.MUL)]
    [TestCase("/", TokenType.DIV)]
    [TestCase("(", TokenType.LPAREN)]
    [TestCase(")", TokenType.RPAREN)]
    [TestCase("abs", TokenType.ABS)]
    public void Should_Succeed_For_Valid_Non_Numeric_Tokens(string text, TokenType type)
    {
      ExpectSingleToken(text, type);
    }

    [Test]
    public void Numbers_Should_Produce_REAL()
    {
      ExpectSingleToken("0", TokenType.REAL);
      ExpectSingleToken("1", TokenType.REAL);
      ExpectSingleToken("123", TokenType.REAL);
      ExpectSingleToken("1234567890123456", TokenType.REAL);
      ExpectSingleToken("1234567890123.456", TokenType.REAL);
    }

    [Test]
    public void Invalid_Chars_Should_Throw_Exception()
    {
      string alphabeth = @"abcdefghijklmnopqrstuvwxyz=\_АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ[]";

      for( var i = 0; i < alphabeth.Length; ++i)
      {
        var c = alphabeth.Substring(i, 1); 
        var lexer = new Lexer(c);

          Assert.That(
              () => lexer.MoveNext(),
              Throws.InstanceOf(typeof(InvalidTokenException)).With.Property("Location").EqualTo(0),
              $"Char {c} should have thrown InvalidTokenException!"
            );
      }
    }

    [Test]
    public void Should_Tokenize_Expression_With_Whitespace()
    {
      ExpectTokens("2.5 * 2 + 7\t/ abs(-2 - 5) / 7",
        new Token[] {
          new Token(TokenType.REAL,   "2.5", 0),
          new Token(TokenType.MUL,    "*",   4),
          new Token(TokenType.REAL,   "2",   6),
          new Token(TokenType.PLUS,   "+",   8),
          new Token(TokenType.REAL,   "7",   10),
          new Token(TokenType.DIV,    "/",   12),
          new Token(TokenType.ABS,    "abs", 14),
          new Token(TokenType.LPAREN, "(",   17),
          new Token(TokenType.MINUS,  "-",   18),
          new Token(TokenType.REAL,   "2",   19),
          new Token(TokenType.MINUS,  "-",   21),
          new Token(TokenType.REAL,   "5",   23),
          new Token(TokenType.RPAREN, ")",   24),
          new Token(TokenType.DIV,    "/",   26),
          new Token(TokenType.REAL,   "7",   28),
        }
        );
    }

    [Test]
    public void Invalid_Real_Number_Should_Produce_1_Token_And_Throw_Exception()
    {
      var text = "123..456";
      var lexer = new Lexer(text);

      Assert.That(lexer.MoveNext(), Is.True);
      ExpectToken(lexer, new Token(TokenType.REAL, "123", 0));

      Assert.That( () => lexer.MoveNext(),
        Throws.InstanceOf(typeof(InvalidTokenException))
        );
    }

    /// <summary>
    /// Tests that given the input text, Lexer outputs
    /// the epxected tokens followed by the EOF token
    /// </summary>
    /// <param name="inputText"></param>
    /// <param name="expectedTokens"></param>
    private void ExpectTokens(string inputText, Token[] expectedTokens)
    {
      ExpectTokens(new Lexer(inputText), expectedTokens);
    }

    #region shared utility functions
    internal static void ExpectToken(IEnumerator<Token> tokens, Token expectedToken)
    {
      Assert.That(tokens.Current.TokenType, Is.EqualTo(expectedToken.TokenType),
        $"Expected {expectedToken.TokenType} at location {expectedToken.Location}");
      Assert.That(tokens.Current.Text, Is.EqualTo(expectedToken.Text),
        $"Expected {expectedToken.TokenType} with text {expectedToken.Text} at location {expectedToken.Location}");
      Assert.That(tokens.Current.Location, Is.EqualTo(expectedToken.Location));
    }

    /// <summary>
    /// Uses Lexer to test that the input string contains
    /// only a single token of the expected type followed by the EOF token
    /// </summary>
    /// <param name="tokenText"></param>
    /// <param name="expectedType"></param>
    internal static void ExpectSingleToken(string tokenText, TokenType expectedType)
    {
      var lexer = new Lexer(tokenText);

      Assert.That(lexer.MoveNext(), Is.True);
      ExpectToken(lexer, new Token(expectedType, tokenText, 0));

      Assert.That(lexer.MoveNext(), Is.True);
      ExpectToken(lexer, new Token(TokenType.EOF, string.Empty, tokenText.Length));
    }

    /// <summary>
    /// Tests that the specified Lexer outputs
    /// the epxected tokens followed by the EOF token
    /// </summary>
    /// <param name="tokens"></param>
    /// <param name="expectedTokens"></param>
    internal static void ExpectTokens(IEnumerator<Token> tokens, Token[] expectedTokens)
    {
      for (int i = 0; i < expectedTokens.Length; ++i)
      {
        Assert.That(tokens.MoveNext(), Is.True);
        ExpectToken(tokens, expectedTokens[i]);
      }
      Assert.That(tokens.MoveNext(), Is.True);
      Assert.That(tokens.Current.TokenType, Is.EqualTo(TokenType.EOF));
    }
    #endregion

  }
}
