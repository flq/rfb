using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  [DebuggerDisplay("Variable: {VariableName} - {Value}")]
  public class PSWithReturnValueToken : AbstractDefinedValueToken
  {
    public PSScriptReturnValueType ValueType { get; private set; }

    public override IToken Clone()
    {
      return new PSWithReturnValueToken { Value = Value, VariableName = VariableName, ValueType = ValueType};
    }

    protected override void withinHandle(TokenizerHandle tHandle)
    {
      var typeOfOutput = tHandle.CurrentLine.TrimStart(' ').Substring(0, 1);
      if (typeOfOutput == "@")
        ValueType = PSScriptReturnValueType.ItemGroup;
      else if (typeOfOutput == "$")
        ValueType = PSScriptReturnValueType.Property;
      else
        throw new InvalidOperationException("Impossible! No way!");
    }

    protected override void reset()
    {
      base.reset();
      ValueType = PSScriptReturnValueType.Undefined;
    }

    public override void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }

    protected override Regex matchRegEx
    {
      get { return KnownRegularExpressions.CapturingVariableMatch; }
    }
  }
}