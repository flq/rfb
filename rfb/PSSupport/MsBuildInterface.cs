using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace rfb.PSSupport
{
  public class MsBuildInterface : PSHostUserInterface
  {
    private TaskLoggingHelper taskLoggingHelper;

    public void SetMsBuildLogger(TaskLoggingHelper msBuildLogger)
    {
      taskLoggingHelper = msBuildLogger;
    }

    public void RemoveMsBuildLogger()
    {
      taskLoggingHelper = null;
    }

    public override string ReadLine()
    {
      return Console.ReadLine();
    }

    public override void Write(string value)
    {
      taskLoggingHelper.LogMessage(MessageImportance.Normal, value);
    }

    public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
    {
      //MsBuild hasn't got this nice control over things, so we'll map yellow/red to warning/error
      // and ignore all others
      if (foregroundColor.Equals(ConsoleColor.Red))
        taskLoggingHelper.LogError(value);
      else if (foregroundColor.Equals(ConsoleColor.Yellow))
        taskLoggingHelper.LogWarning(value);
      else
        taskLoggingHelper.LogMessage(MessageImportance.High, value);
    }

    public override void WriteLine(string value)
    {
      taskLoggingHelper.LogMessage(MessageImportance.Normal, value);
    }

    public override void WriteErrorLine(string value)
    {
      taskLoggingHelper.LogError(value);
    }

    public override void WriteDebugLine(string message)
    {
      taskLoggingHelper.LogMessage(MessageImportance.Low, message);
    }

    public override void WriteProgress(long sourceId, ProgressRecord record)
    {
      taskLoggingHelper.LogMessage(MessageImportance.Normal, "Powershell with SourceID {0} notifies of progress: {1}", record.ToString());
    }

    public override void WriteVerboseLine(string message)
    {
      taskLoggingHelper.LogMessage(MessageImportance.Low, message);
    }

    public override void WriteWarningLine(string message)
    {
      taskLoggingHelper.LogWarning(message);
    }

    public override SecureString ReadLineAsSecureString()
    {
      throw new NotImplementedException("No support for reading secured string");
    }

    public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
    {
      throw new InvalidOperationException("A prompt is currently not supported");
    }

    public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
    {
      throw new InvalidOperationException("prompting for credentials is currently not supported");
    }

    public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
    {
      throw new InvalidOperationException("prompting for credentials is currently not supported");
    }

    public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
    {
      throw new InvalidOperationException("prompting for choice is currently not supported");
    }

    public override PSHostRawUserInterface RawUI
    {
      get { return new PSRawUserItf(); }
    }
  }
}