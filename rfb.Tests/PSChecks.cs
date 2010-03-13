using NUnit.Framework;
using rfb.PSSupport;
using rfb.Tests.Utils;

namespace rfb.Tests
{
  [TestFixture,Ignore]
  public class PSChecks
  {
    private readonly PowershellRunner r = PowershellRunner.Me;

    [Test,Ignore]
    public void ItIsPossibleToExecuteAScript()
    {
      var objects = r.InvokeScript("$a = $env:path; $a.Split(\";\")");
      objects.Count.ShouldBeGreaterThan(0);
    }
  }
}