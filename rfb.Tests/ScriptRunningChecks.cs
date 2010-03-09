using NUnit.Framework;
using rfb.Tests.Utils;

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
    public void SetupAllowsToPassInProperties()
    {
      var runner = new ScriptRunner()
        .SetFile("_scriptWithVarOutput.txt")
        .ExposeSetup(b => b.Property = "Foo=Baz;Bar=Alice")
        .Run();
      runner.Logged("Baz").ShouldBeTrue();
      runner.Logged("Alice").ShouldBeTrue();
    }
  }
}