using System;
using System.IO;
using NUnit.Framework;
using rfb.Tests.Utils;
using rfb.Token;
using System.Linq;

namespace rfb.Tests
{
  [TestFixture]
  public class TokenizerStreamChecks
  {
    [Test]
    public void IntegrativeCheckOnASensibleProject()
    {
      var t = "sensibleProject1.txt".Tokenized();
      var vtor = new AssertingTokenStreamVisitor();
      t.Accept(vtor);
      vtor.TotalCount.ShouldBeEqualTo(18);
      vtor.VisitedProjectToken.ShouldBeEqualTo(1);
      vtor.VisitedUsingTaskToken.ShouldBeEqualTo(1);
      vtor.VisitedTargetToken.ShouldBeEqualTo(2);
      vtor.VisitedVariableToken.ShouldBeEqualTo(2);
      vtor.VisitedBacktick.ShouldBeEqualTo(1);
      vtor.VisitedExternalPsScriptToken.ShouldBeEqualTo(2);
      vtor.VisitedInlineScriptToken.ShouldBeEqualTo(1);
      vtor.VisitedPSWithReturnValueToken.ShouldBeEqualTo(2);
      vtor.VisitedPSScriptCallToken.ShouldBeEqualTo(1);
      vtor.Tokens.OfType<PSWithReturnValueToken>()
        .SingleOrDefault(v=>v.ValueType == PSScriptReturnValueType.Property)
        .ShouldNotBeNull();
      vtor.Tokens.OfType<PSWithReturnValueToken>()
        .SingleOrDefault(v => v.ValueType == PSScriptReturnValueType.ItemGroup)
        .ShouldNotBeNull();
      vtor.Tokens.Last().ShouldBeOfType<EndToken>();
    }

    [Test]
    public void CorrectParsingOfProjectWithScript()
    {
      var t = "_scriptWithPowershell.txt".Tokenized();
      var vtor = new AssertingTokenStreamVisitor();
      t.Accept(vtor);
      vtor.VisitedAnyToken.ShouldBeEqualTo(1);
      vtor.VisitedPSWithReturnValueToken.ShouldBeEqualTo(1);
      vtor.VisitedEndToken.ShouldBeEqualTo(1);
    }

    [Test]
    public void CorrectParsingOfOptions()
    {
      var t = "_scriptManyTasksWithOptions.txt".Tokenized();
      var vtor = new AssertingTokenStreamVisitor();
      t.Accept(vtor);
      vtor.VisitedEndToken.ShouldBeEqualTo(1);
      vtor.VisitedAnyToken.ShouldBeEqualTo(3);
    }

    [Test]
    public void UsingSameTokenSeveralTimes()
    {
      var t = "twotasks.txt".Tokenized();
      var vtor = new AssertingTokenStreamVisitor();
      t.Accept(vtor);
      vtor.VisitedAnyToken.ShouldBeEqualTo(3);
    }

    [Test]
    public void ItemGroupRecognition()
    {
      var t = "itemGroups.txt".Tokenized();
      var vtor = new AssertingTokenStreamVisitor();
      t.Accept(vtor);
      vtor.VisitedItemGroupToken.ShouldBeEqualTo(2);
    }

    [Test]
    public void IssuesCausedByRfbBuildScript()
    {
      var t = "_rfbCopyFollowedByPSCall.txt".Tokenized();
      var vtor = new AssertingTokenStreamVisitor();
      t.Accept(vtor);
      vtor.VisitedTargetToken.ShouldBeEqualTo(1);
      vtor.VisitedPSScriptCallToken.ShouldBeEqualTo(1);
    }
  }
}