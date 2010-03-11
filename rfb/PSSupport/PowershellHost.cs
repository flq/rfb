using System;
using System.Globalization;
using System.Management.Automation.Host;

namespace rfb
{
  public class PowershellHost : PSHost
  {
    public override void SetShouldExit(int exitCode)
    {
      throw new NotImplementedException();
    }

    public override void EnterNestedPrompt()
    {
      throw new NotImplementedException();
    }

    public override void ExitNestedPrompt()
    {
      throw new NotImplementedException();
    }

    public override void NotifyBeginApplication()
    {
      throw new NotImplementedException();
    }

    public override void NotifyEndApplication()
    {
      throw new NotImplementedException();
    }

    public override string Name
    {
      get { throw new NotImplementedException(); }
    }

    public override Version Version
    {
      get { throw new NotImplementedException(); }
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