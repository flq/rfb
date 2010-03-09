using System;

namespace rfb.Tests.Utils
{
  public class ScriptRunner
  {
    private readonly BuilderSetup setup;

    public ScriptRunner()
    {
      setup = new BuilderSetup
                {
                  LoggerType = typeof (AssertingMsBuildLogger).AssemblyQualifiedName
                };
    }

    public ScriptRunner SetFile(string file)
    {
      if (file == null) throw new ArgumentNullException("file");
      setup.BuildData = DataMother.GetStream(file);
      return this;
    }

    public ScriptRunner ExposeSetup(Action<BuilderSetup> setupAction)
    {
      setupAction(setup);
      return this;
    }

    public ScriptRunner Run()
    {
      var runner = new BuildRunner(setup);
      runner.Run();
      return this;
    }

    public AssertingMsBuildLogger Logger
    {
      get { return AssertingMsBuildLogger.me; }
    }

    public bool Logged(string message)
    {
      return Logger.Logged(message);
    }
  }
}