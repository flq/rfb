using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  [DebuggerDisplay("Variable: {VariableName} - {Value}")]
  public class VariableToken : AbstractDefinedValueToken
  {
    private static readonly Regex variableMatch = new Regex(@"\$(\w+)\s*=\s*(.+)$", RegexOptions.Compiled);

    public override IToken Clone()
    {
      return new VariableToken {Value = Value, VariableName = VariableName};
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override Regex matchRegEx
    {
      get { return variableMatch; }
    }
  }
}