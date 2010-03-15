using NUnit.Framework;
using rfb.Token;
using rfb.Tests.Utils;

namespace rfb.Tests
{
  [TestFixture]
  public class TokenChecks
  {
    [Test]
    public void ProjectTokenWorksAsExpected()
    {
      var handle = new TokenizerHandle("Project \"Main\"");
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();
      
      t.ShouldNotBeNull();
      t.Options.Count.ShouldBeEqualTo(1);
      t.Word.ShouldBeEqualTo("Project");
    }

    [Test]
    public void NodeWithOptionsHasIndexer()
    {
      var handle = new TokenizerHandle(DataMother.GetEmptyProjectFile());
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();
      t["DefaultTargets"].ShouldBeEqualTo("Main");
      t["Bla"].ShouldBeNull();
    }

    [Test]
    public void NodeWithOptionsEscapesDefault()
    {
      var handle = new TokenizerHandle(DataMother.GetFile("node with default needs escaping.txt"));
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();
      t["Default"].ShouldBeEqualTo("Default stuff \" Hoho");
      t.Options.Count.ShouldBeEqualTo(4);
    }

    [Test]
    public void BacktickTokenWorks()
    {
      var handle = new TokenizerHandle(DataMother.GetFile("backtick check.txt"));
      handle.FastForwardToLine(3);
      var t = handle.ProcessWithToken<BacktickToken>();
      t.BackTickValue.ShouldBeEqualTo("echo y| cacls %(Binaries.Identity) /G everyone:R");
    }

    [Test]
    public void DefaultOptionMapping()
    {
      var handle = new TokenizerHandle(DataMother.NodeWithOptionsVariantB());
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();
      t.MapDefaultTo("Option1");
      t["Default"].ShouldBeNull();
      t["Option1"].ShouldNotBeNull();
    }

    [Test]
    public void ProjectTokenWorksAsExpectedVariantB()
    {
      var handle = new TokenizerHandle(DataMother.NodeWithOptionsVariantB());
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();

      t.ShouldNotBeNull();
      t.Options.Count.ShouldBeEqualTo(3);
    }

    [Test]
    public void NodeWithOptionsWorksAsExpected()
    {
      var handle = new TokenizerHandle(DataMother.GetNodeWithOptions());
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();
      
      t.ShouldNotBeNull();
      t.Options.ShouldHaveCount(4);
      t.Options["Default"].StartsWith("Default").ShouldBeTrue();
      t.Options["Option1"].ShouldBeEqualTo("Bla di Bla");
      t.Options["Option3"].ShouldBeEqualTo("Blablub");
    }

    [Test]
    public void TargetTokenWorksAsExpected()
    {
      var handle = new TokenizerHandle(DataMother.ProjectAndTarget());
      handle.FastForwardToLine(3);

      var t = handle.ProcessWithToken<TargetToken>();
      t.ShouldNotBeNull();
      t.Options.ShouldHaveCount(2);
      t.Options["Default"].StartsWith("Default").ShouldBeTrue();
      t.Options["Depends"].StartsWith("Deps").ShouldBeTrue();
    }

    [Test]
    public void ProjectTokenDoesNotGobbleOptionsOfTargetToken()
    {
      var handle = new TokenizerHandle(DataMother.ProjectAndTarget());
      handle.FastForwardToLine(1);
      var t = handle.ProcessWithToken<ProjectToken>();
      t.Options.ShouldHaveCount(1);
    }

    [Test]
    public void VariableTokenWorksAsExpected()
    {
      var handle = new TokenizerHandle(DataMother.ProjectAndTarget());
      handle.FastForwardToLine(2);
      var t = handle.ProcessWithToken<VariableToken>();
      t.ShouldNotBeNull();
      t.VariableName.ShouldBeEqualTo("Foo");
      t.Value.ShouldBeEqualTo("Bar Baz");
    }

    [Test]
    public void PowershellScriptToken()
    {
      var handle = new TokenizerHandle(DataMother.GetFile("_scriptWithPowershell.txt"));
      handle.FastForwardToLine(7);
      var t = handle.ProcessWithToken<PSScriptToken>();
      t.ScriptName.ShouldBeEqualTo("smallPNGs");
      t.Script.IndexOf("echo").ShouldBeSmallerThan(5);
      t.Script.Contains("FullName").ShouldBeTrue();
      t.Terminator.ShouldBeEqualTo("END");
    }

    [Test]
    public void PowershellScriptCallTokenNotMatchingScriptDef()
    {
      var handle = new TokenizerHandle(DataMother.GetFile("_scriptWithPowershell.txt"));
      handle.FastForwardToLine(7);
      var t = handle.ProcessWithToken<PSScriptCallToken>();
      t.ShouldBeNull();
    }

    [Test]
    public void PowershellScriptCallToken()
    {
      var handle = new TokenizerHandle(DataMother.GetFile("_scriptWithPowershellExecAsTask.txt"));
      handle.FastForwardToLine(4);
      var t = handle.ProcessWithToken<PSScriptCallToken>();
      t.ShouldNotBeNull();
      t.Word.ShouldBeEqualTo("MakeFile");
    }
  }
}