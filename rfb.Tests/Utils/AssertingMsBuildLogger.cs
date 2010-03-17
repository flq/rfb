using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace rfb.Tests.Utils
{
  public class AssertingMsBuildLogger : ILogger
  {
    public static readonly AssertingMsBuildLogger me = new AssertingMsBuildLogger(); 

    public List<string> LoggedMessages { get; private set; }

    public bool Logged(string message)
    {
      return LoggedMessages.Contains(message);
    }

    void ILogger.Initialize(IEventSource eventSource)
    {
      me.LoggedMessages = new List<string>();
      eventSource.MessageRaised += me.handleMessageRaised;
      eventSource.WarningRaised += me.handleWarningRaised;
    }

    private void handleMessageRaised(object sender, BuildMessageEventArgs e)
    {
      LoggedMessages.Add(e.Message);
      Console.WriteLine(e.Message);
    }

    private void handleWarningRaised(object sender, BuildWarningEventArgs e)
    {
      LoggedMessages.Add(e.Message);
      Console.WriteLine(e.Message);
    }

    void ILogger.Shutdown()
    {
      
    }

    LoggerVerbosity ILogger.Verbosity { get; set; }

    public string Parameters { get; set; }
  }
}