using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class PSInlineScriptToken : PSScriptToken
  {
    private static readonly Regex psScriptStart = new Regex(@"\s*PS\s*<<(\w+)", RegexOptions.Compiled);

    protected override AbstractToken handle(TokenizerHandle handle)
    {
      var match = psScriptStart.Match(handle.CurrentLine);
      if (match.Groups.Count != 2)
        return null;
      Terminator = match.Groups[1].Value;
      extractScript(handle);
      handle.CurrentHandled();
      return this;
    }

    public override IToken Clone()
    {
      return new PSInlineScriptToken {Script = Script, Terminator = Terminator};
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}