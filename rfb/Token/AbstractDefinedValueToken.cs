using System;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public abstract class AbstractDefinedValueToken : AbstractToken
  {
    protected override AbstractToken handle(TokenizerHandle tHandle)
    {
      var m = matchRegEx.Match(tHandle.CurrentLine);
      if (m.Groups.Count == 0 || m.Groups.Count != 3)
        return null;

      VariableName = m.Groups[1].Captures[0].Value.Trim(' ');
      Value = m.Groups[2].Captures[0].Value.Trim(' ');
      tHandle.CurrentHandled();
      return this;
    }

    public string Value { get; protected set; }
    public string VariableName { get; protected set; }

    protected abstract Regex matchRegEx { get; }

    protected override void reset()
    {
      Value = null;
      VariableName = null;
    }
  }
}