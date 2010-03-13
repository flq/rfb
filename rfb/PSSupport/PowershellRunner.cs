using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace rfb.PSSupport
{
  public class PowershellRunner : IDisposable
  {
    private readonly Runspace runSpace;

    private static PowershellRunner me;

    public static PowershellRunner Me
    {
      get
      {
        return me ?? (me = new PowershellRunner());
      }
    }
    
    private PowershellRunner()
    {
      runSpace = RunspaceFactory.CreateRunspace();
      runSpace.Open();
    }

    public Collection<PSObject> InvokeScript(string script)
    {
      var p = runSpace.CreatePipeline();
      var cmd = new Command(script, true);
      p.Commands.Add(cmd);
      return p.Invoke();
    }

    public void Dispose()
    {
      runSpace.Dispose();
    }
  }
}