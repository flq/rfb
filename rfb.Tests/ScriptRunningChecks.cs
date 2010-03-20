using System;
using System.IO;
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

    [Test]
    public void RunningAScript()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithPowershellExecAsTask.txt")
        .Run();
      var createdFile = Path.Combine(Path.GetTempPath(), "testtest.tst");
      File.Exists(createdFile).ShouldBeTrue();
      File.Delete(createdFile);
    }

    [Test]
    public void UsingParametersForPowershell()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithParams.txt")
        .Run();
      var msg = runner.Logger.LoggedMessages.Find(s => s.Contains("Swonk"));
      msg.ShouldNotBeNull();
      msg.ShouldBeEqualTo("We have Foodadl and Swonk");
    }

    [Test]
    public void PowershellLogsArriveInMsBuild()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptPowershellWritingHost.txt")
        .Run();
      var msg = runner.Logger.LoggedMessages.Find(s => s.Contains("Hi"));
      msg.ShouldNotBeNull();
      msg = runner.Logger.LoggedMessages.Find(s => s.Contains("Ho"));
      msg.ShouldNotBeNull();
    }

    [Test]
    public void PowershellInlineScriptsWork()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithPowershellInline.txt")
        .Run();
      var msg = runner.Logger.LoggedMessages.Find(s => s.Contains("Inside the script block"));
      msg.ShouldNotBeNull();
    }

    [Test]
    public void ItemGroupUsage()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptPowershellExampleGitUsage.txt")
        .Run();

      var msg = runner.Logger.LoggedMessages.Find(s => s.Contains("gitnumber"));
      msg.ShouldNotBeNull();
      msg.Length.ShouldBeGreaterThan(19); //That would be if the vars are empty
    }
  }
}