using NUnit.Framework;
using rfb.Tests.Utils;

namespace rfb.Tests
{
  [TestFixture]
  public class MsBuildProjectBuilderChecks
  {
    readonly MsBuildProjectBuilder builder = new MsBuildProjectBuilder(new StandardDefaultValueResolver());

    [Test]
    public void ApplyingDefaultTargetOnProject()
    {
      var t = "sensibleProject1.txt".Tokenized();
      t.Accept(builder);
      builder.Project.ToolsVersion.ShouldBeEqualTo("3.5");
      builder.Project.DefaultTargets.ShouldBeEqualTo("Default");
    }
  }
}