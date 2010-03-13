using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace rfb.Token
{
  public class Tokenizer :IEnumerable<IToken>
  {
    private readonly TokenizerHandle handle;
    private readonly List<IToken> tokens;

    public Tokenizer(TextReader reader)
    {
      handle = new TokenizerHandle(reader);
      tokens = new List<IToken>
                 {
                   new ProjectToken(),
                   new TargetToken(),
                   new PsScriptToken(),
                   new UsingTaskToken(),
                   new VariableToken(),
                   new PSWithReturnValueToken(),
                   new ItemGroupToken(),
                   new CommentToken(),
                   new BacktickToken(),
                   new AnyToken()
                 };
    }

    public IEnumerator<IToken> GetEnumerator()
    {
      var shouldBeKnockedOut = false;
      while (handle.Advance() != null)
      {
        var usefulTokens =
          (from t in tokens
          let usefulToken = t.Check(handle)
          where usefulToken != null
          select usefulToken).ToList();
        foreach (var t in usefulTokens)
          yield return t;
        if (usefulTokens.Count > 0)
          shouldBeKnockedOut = false;
        if (!handle.IsCurrentHandled)
        {
          if (!shouldBeKnockedOut)
            shouldBeKnockedOut = true;
          else
          {
            handle.CurrentHandled();
            shouldBeKnockedOut = false;
          }
        }
        else
        {
          shouldBeKnockedOut = false;
        }
      }
      yield return new EndToken();
    }

    public void Accept(ITokenStreamVisitor vtor)
    {
      foreach (var t in this)
        t.Accept(vtor);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}