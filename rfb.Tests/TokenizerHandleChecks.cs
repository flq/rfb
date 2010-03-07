using System;
using NUnit.Framework;
using rfb.Tests.Utils;

namespace rfb.Tests
{
  [TestFixture]
  public class TokenizerHandleChecks
  {
    [Test]
    public void TokenHandleReturnsEmptyStringOnceItIsRunOut()
    {
      var th = new TokenizerHandle("hello" + Environment.NewLine + "world");
      th.Advance();
      th.CurrentLine.ShouldBeEqualTo("hello");
      th.CurrentHandled();
      th.Advance();
      th.CurrentLine.ShouldBeEqualTo("world");
      th.CurrentHandled();
      th.Advance();
      th.CurrentLine.ShouldBeNull();
    }
  }
}