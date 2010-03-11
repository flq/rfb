using System;
using System.Text;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public class PsScriptToken : AbstractToken
  {
    private static Regex psScriptStart = new Regex(@"\s*PS\s""(.+)""\s*<<(\w+)", RegexOptions.Compiled);

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

    private void extractScript(TokenizerHandle h)
    {
      h.CurrentHandled();
      h.Advance();
      var rEnd = new Regex(@"\s*" + Terminator);
      StringBuilder b = new StringBuilder();
      while (!rEnd.IsMatch(h.CurrentLine))
      {
        b.AppendLine(h.CurrentLine);
        h.CurrentHandled();
        h.Advance();
      }
      Script = b.ToString();
    }

    public string Script { get; private set; }
    public string ScriptName { get; private set; }
    public string Terminator { get; private set; }

    protected override void reset()
    {
      Script = null;
      ScriptName = null;
      Terminator = null;
    }

    public override IToken Clone()
    {
      return new PsScriptToken { Script = Script, ScriptName = ScriptName, Terminator = Terminator};
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      
    }
  }
}