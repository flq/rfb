using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.PowerShell.Commands;

namespace rfb.PSSupport
{

  public static class PowershellRunspace
  {
    private static readonly Runspace runSpace;
    private static readonly MsBuildInterface itfToMsBuild;

    static PowershellRunspace()
    {
      itfToMsBuild = new MsBuildInterface();
      runSpace = RunspaceFactory.CreateRunspace(new PowershellHost(itfToMsBuild));
      runSpace.Open();
    }

    public static PowerShellRunner CreateRunner(TaskLoggingHelper msBuildLogger)
    {
      return new PowerShellRunner(msBuildLogger);
    }

    public class PowerShellRunner : IDisposable
    {
      public PowerShellRunner(TaskLoggingHelper msBuildLogger)
      {
        itfToMsBuild.SetMsBuildLogger(msBuildLogger);
      }

      public Collection<PSObject> InvokeScript(string script)
      {
        return InvokeScript(script, null);
      }

      public Collection<PSObject> InvokeScript(string script, IDictionary parameters)
      {
        var p = runSpace.CreatePipeline();
        var cmd = new Command(script, true);
        if (parameters != null)
          foreach (DictionaryEntry kv in parameters)
            cmd.Parameters.Add(kv.Key.ToString(), kv.Value);

        p.Commands.Add(cmd);
        return p.Invoke();
      }

      public void Dispose()
      {
        itfToMsBuild.RemoveMsBuildLogger();
      }
    }
  }
}