using System;
using System.Globalization;
using System.Management.Automation.Host;
using Microsoft.Build.Framework;

namespace rfb.PSSupport
{
  public class PowershellHost : PSHost
  {
    private readonly MsBuildInterface msBuildInterface;
    private readonly Guid guid;

    public PowershellHost(MsBuildInterface msBuildInterface)
    {
      this.msBuildInterface = msBuildInterface;
      guid = Guid.NewGuid();
    }

    public override void SetShouldExit(int exitCode)
    {
      
    }

    public override void EnterNestedPrompt()
    {
      throw new NotImplementedException("Nested prompt not supported by rfb PS Host");
    }

    public override void ExitNestedPrompt()
    {
      throw new NotImplementedException("Nested prompt not supported by rfb PS Host");
    }

    public override void NotifyBeginApplication()
    {
      
    }

    public override void NotifyEndApplication()
    {
      
    }

    public override string Name
    {
      get { return "rfb PS Host"; }
    }

    public override Version Version
    {
      get { return new Version(1, 0, 0, 0); }
    }

    public override Guid InstanceId
    {
      get { return guid; }
    }

    public override PSHostUserInterface UI
    {
      get { return msBuildInterface; }
    }

    public override CultureInfo CurrentCulture
    {
      get { return CultureInfo.InvariantCulture; }
    }

    public override CultureInfo CurrentUICulture
    {
      get { return CultureInfo.InstalledUICulture; }
    }
  }
}