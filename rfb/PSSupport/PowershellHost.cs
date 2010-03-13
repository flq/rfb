using System;
using System.Globalization;
using System.Management.Automation.Host;

namespace rfb
{
  public class PowershellHost : PSHost
  {
    

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
      get { throw new NotImplementedException(); }
    }

    public override PSHostUserInterface UI
    {
      get { throw new NotImplementedException(); }
    }

    public override CultureInfo CurrentCulture
    {
      get { throw new NotImplementedException(); }
    }

    public override CultureInfo CurrentUICulture
    {
      get { throw new NotImplementedException(); }
    }
  }
}