using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class BacktickToken : AbstractToken
  {
    private static readonly Regex matchBacktick = new Regex(@"^\s*`(.+)");

    protected override AbstractToken handle(TokenizerHandle handle)
    {
      var match = matchBacktick.Match(handle.CurrentLine);
      if (match.Groups.Count > 1)
      {
        BackTickValue = match.Groups[1].Value;
        handle.CurrentHandled();
        return this;
      }
      return null;
    }

    public string BackTickValue { get; private set; }

    protected override void reset()
    {
      BackTickValue = null;
    }

    public override IToken Clone()
    {
      return new BacktickToken {BackTickValue = BackTickValue};
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}