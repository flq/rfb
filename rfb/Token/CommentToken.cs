using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class CommentToken : AbstractToken
  {
    private static readonly Regex commentMatch = new Regex(@"^[ \t]*\/\/.*$", RegexOptions.Compiled);


    protected override AbstractToken handle(TokenizerHandle handle)
    {
      if (!commentMatch.IsMatch(handle.CurrentLine))
        return null;
      handle.CurrentHandled();
      return this;
    }

    public override IToken Clone()
    {
      return new CommentToken();
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override void reset() { }
  }
}