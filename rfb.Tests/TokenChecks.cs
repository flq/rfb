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
  }
}