using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using rfb.PSSupport;
using System.Linq;

namespace rfb.MsBuildTasks
{
  public class RunScript : Task
  {
    [Required]
    public string Script { get; set; }

    [Output]
    public ITaskItem[] ScriptItemOutput { get; set; }

    [Required]
    public string Capture { get; set; }

    [Output]
    public string ScriptPropOutput { get; set; }

    public override bool Execute()
    {
      Log.LogMessage(MessageImportance.Normal, "About to run Powershell script");
      var output = PowershellRunner.Me.InvokeScript(Script);

      var membersToGet = Capture.Split(',').Select(s => s.Trim(' ')).ToList();

      ScriptItemOutput =
        (from o in output
        let matchingMembers =
          (from m in membersToGet
           let prop = o.Properties.Match(m).FirstOrDefault()
           where prop != null
           select new {prop.Name, prop.Value}).ToList()
        where matchingMembers.Count > 0
        select
          new TaskItem(matchingMembers[0].Value.ToString(),
                       matchingMembers.Skip(1).ToDictionary(a => a.Name, a => a.Value))).ToArray();

      return true;
    }
  }
}