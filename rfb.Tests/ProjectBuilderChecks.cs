using System;
using System.IO;
using NUnit.Framework;
using rfb.Tests.Utils;

namespace rfb.Tests
{
  [TestFixture]
  public class ProjectBuilderChecks
  {
    [Test]
    public void RfSiteScriptLookAt()
    {
      var t = "rfSiteBuildScript.txt".Tokenized();
      var b = new MsBuildProjectBuilder(new StandardDefaultValueResolver());
      t.Accept(b);
      var sw = new StringWriter();
      b.Project.Save(sw);

      var project = sw.GetStringBuilder().ToString();
      Console.WriteLine(project);

      var expected = DataMother.GetFile("rfSiteBuildScript.generated.txt");
      project.ShouldBeEqualTo(expected);
    }
    
  }
}