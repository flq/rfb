using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class UsingTaskToken : TokenWithOptions
  {
    private readonly static Regex startsWithUsingTask = new Regex(@"^\s{2}UsingTask", RegexOptions.Compiled);

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override Regex matchCondition
    {
      get { return startsWithUsingTask; }
    }
  }
}