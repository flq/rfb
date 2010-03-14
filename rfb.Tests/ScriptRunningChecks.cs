using System;
using NUnit.Framework;
using rfb.Tests.Utils;
using System.Linq;

namespace rfb.Tests
{
  [TestFixture]
  public class ScriptRunningChecks
  {
    [Test]
    public void MessagesAreObtainableFromTheScript()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithMessage.txt")
        .Run();
      runner.Logged("Hello from Default").ShouldBeTrue();
    }

    [Test]
    public void SetupAllowsToSepcifyATarget()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithMessage.txt")
        .ExposeSetup(b=>b.Target="B")
        .Run();
      runner.Logged("Hello from Default").ShouldBeFalse();
      runner.Logged("Hello from B").ShouldBeTrue();
    }

    [Test]
    public void AForeachLoopInAScript()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithIteration.txt")
        .Run();
      runner.Logged("1").ShouldBeTrue();
      runner.Logged("2").ShouldBeTrue();
      runner.Logged("3").ShouldBeTrue();
    }

    [Test]
    public void SetupAllowsToPassInProperties()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithVarOutput.txt")
        .ExposeSetup(b => b.Property = "Foo=Baz;Bar=Alice")
        .Run();
      runner.Logged("Baz").ShouldBeTrue();
      runner.Logged("Alice").ShouldBeTrue();
    }

    [Test]
    public void ConnectingItemGroupToPSOutput()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithPowershell.txt")
        .Run();
      runner.Logger.LoggedMessages.Count(m=>m.EndsWith(".png")).ShouldBeGreaterThan(10);
    }

    [Test]
    public void ConnectingPropertyToPSOutput()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithPowershellFillProp.txt")
        .Run();
      runner.Logged(DateTime.Now.ToString("yyyy.MM.dd")).ShouldBeTrue();
    }
  }
}