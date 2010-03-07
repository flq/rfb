using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  [DebuggerDisplay("Target: {Options[\"Default\"]}")]
  public class TargetToken : TokenWithOptions
  {
    private readonly static Regex startsWithTarget = new Regex(@"^\s{2}Target", RegexOptions.Compiled);

    protected override Regex matchCondition
    {
      get { return startsWithTarget; }
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}