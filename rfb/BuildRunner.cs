using System;
using System.IO;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using rfb.Token;

namespace rfb
{
  public class BuildRunner : IDisposable
  {
    private readonly BuilderSetup setup;
    private readonly Engine engine;

    public BuildRunner(BuilderSetup setup)
    {
      this.setup = setup;
      engine = new Engine();
      engine.RegisterLogger(new ConsoleLogger(LoggerVerbosity.Minimal));
    }

    public void Dispose()
    {
      engine.Shutdown();
    }

    public void Run()
    {
      using (var tr = File.OpenText(setup.BuildFile))
      {
        var builder = new MsBuildProjectBuilder(new StandardDefaultValueResolver(), engine);
        var tokenizer = new Tokenizer(tr);
        tokenizer.Accept(builder);
        engine.BuildProject(builder.Project);
      }
    }
  }
}