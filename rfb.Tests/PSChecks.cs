using NUnit.Framework;
using rfb.PSSupport;
using rfb.Tests.Utils;

namespace rfb.Tests
{
  [TestFixture]
  public class PSChecks
  {
    private readonly PowershellRunner r = new PowershellRunner();

    [Test]
    public void ItIsPossibleToExecuteAScript()
    {
      var objects = r.InvokeScript("$a = $env:path; $a.Split(\";\")");
      objects.Count.ShouldBeGreaterThan(0);
    }
  }
}