using System.Text;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public abstract class PSScriptToken : AbstractToken
  {
    protected void extractScript(TokenizerHandle h)
    {
      h.CurrentHandled();
      h.Advance();
      var rEnd = new Regex(@"\s*" + Terminator);
      var b = new StringBuilder();
      while (!rEnd.IsMatch(h.CurrentLine))
      {
        b.AppendLine(h.CurrentLine);
        h.CurrentHandled();
        h.Advance();
      }
      Script = b.ToString();
    }

    protected override void reset()
    {
      Script = null;
      Terminator = null;
    }

    public string Script { get; protected set; }
    public string Terminator { get; protected set; }
  }
}