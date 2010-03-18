using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class PSExternalScriptToken : PSScriptToken
  {
    private static readonly Regex psScriptStart = new Regex(@"\s*PS:(\w+)\s*<<(\w+)", RegexOptions.Compiled);

    protected override AbstractToken handle(TokenizerHandle handle)
    {
      var match = psScriptStart.Match(handle.CurrentLine);
      if (match.Groups.Count != 3)
        return null;

      ScriptName = match.Groups[1].Value;
      Terminator = match.Groups[2].Value;
      extractScript(handle);
      handle.CurrentHandled();
      return this;
    }

    public string ScriptName { get; private set; }

    protected override void reset()
    {
      base.reset();
      ScriptName = null;
    }

    public override IToken Clone()
    {
      return new PSExternalScriptToken { Script = Script, ScriptName = ScriptName, Terminator = Terminator};
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}