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
      if (setup.LoggerType != null)
      {
        var logger = (ILogger) Activator.CreateInstance(setup.LoggerAsType);
        engine.RegisterLogger(logger);
      }
      engine.RegisterLogger(new ConsoleLogger(LoggerVerbosity.Minimal));
    }

    public void Dispose()
    {
      engine.Shutdown();
    }

    public void Run()
    {
      using (var tr = new StreamReader(setup.BuildData))
      {
        var builder = new MsBuildProjectBuilder(new StandardDefaultValueResolver(), engine);
        var tokenizer = new Tokenizer(tr);
        tokenizer.Accept(builder);
        if (!string.IsNullOrEmpty(setup.OutputXml))
        {
          builder.Project.Save(setup.OutputXml);
          return;
        }
        if (setup.HasProperties)
          foreach (var p in setup.Properties)
            builder.Project.SetProperty(p.Key, p.Value);
        if (setup.Target != null)
          engine.BuildProject(builder.Project, setup.Target);
        else
          engine.BuildProject(builder.Project);
      }
    }
  }
}