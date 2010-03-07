using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  [DebuggerDisplay("Any: {Word}")]
  public class AnyToken : TokenWithOptions
  {
    protected override Regex matchCondition
    {
      get { return startWord; }
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}