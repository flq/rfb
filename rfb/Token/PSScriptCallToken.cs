using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class PSScriptCallToken : TokenWithOptions
  {
    
    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override Regex startWord
    {
      get
      {
        return KnownRegularExpressions.ScriptCallMatch;
      }
    }

    protected override Regex matchCondition
    {
      get { return KnownRegularExpressions.ScriptCallMatch; }
    }
  }
}