using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  [DebuggerDisplay("ItemGroup: {VariableName} - {Value}")]
  public class ItemGroupToken : AbstractDefinedValueToken
  {
    private static readonly Regex itemGroupMatch = new Regex(@"@(\w+)\s*=\s*(.+)$", RegexOptions.Compiled);

    public override IToken Clone()
    {
      return new ItemGroupToken {Value = Value, VariableName = VariableName};
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override Regex matchRegEx
    {
      get { return itemGroupMatch; }
    }
  }
}