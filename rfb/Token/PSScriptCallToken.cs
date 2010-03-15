using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class PSScriptCallToken : TokenWithOptions
  {
    private static readonly Regex psScriptCall = new Regex(@"\s*PS:(\w+)(?!.*<<)", RegexOptions.Compiled);

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override Regex startWord
    {
      get
      {
        return psScriptCall;
      }
    }

    protected override Regex matchCondition
    {
      get {return psScriptCall; }
    }
  }
}