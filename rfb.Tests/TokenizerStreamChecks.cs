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
      vtor.TotalCount.ShouldBeEqualTo(10);
      vtor.VisitedProjectToken.ShouldBeEqualTo(1);
      vtor.VisitedUsingTaskToken.ShouldBeEqualTo(1);
      vtor.VisitedTargetToken.ShouldBeEqualTo(1);
      vtor.VisitedVariableToken.ShouldBeEqualTo(2);
      vtor.VisitedBacktick.ShouldBeEqualTo(1);
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
  }
}