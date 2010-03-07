using System;
using System.IO;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class ProjectToken : TokenWithOptions
  {
    private readonly static Regex startsWithProject = new Regex(@"^Project", RegexOptions.Compiled);

    protected override Regex matchCondition
    {
      get { return startsWithProject; }
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}