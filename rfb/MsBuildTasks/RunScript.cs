using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
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

    [Required]
    public string ReturnValueType { get; set; }

    [Output]
    public ITaskItem[] ScriptItemOutput { get; set; }

    [Output]
    public string ScriptPropOutput { get; set; }

    public string Capture { get; set; }

    public string CommandParameters { get; set; }

    public override bool Execute()
    {
      Log.LogMessage(MessageImportance.Normal, "About to run Powershell script");

      Dictionary<string, string> @params =
        !string.IsNullOrEmpty(CommandParameters) ? 
          ((KeyValueSerializer) CommandParameters).ToDictionary() : null;

      var output = PowershellRunner.Me.InvokeScript(Script, @params);

      var retValType = (PSScriptReturnValueType)Enum.Parse(typeof(PSScriptReturnValueType), ReturnValueType);

      if (retValType.Equals(PSScriptReturnValueType.ItemGroup))
        ScriptItemOutput = convertOutput(output).ToArray();
      else if (retValType.Equals(PSScriptReturnValueType.Property))
        ScriptPropOutput = convertOutput(output).Last().ItemSpec;
      return true;
    }

    private IEnumerable<TaskItem> convertOutput(IEnumerable<PSObject> output)
    {
      if (Capture == null)
        return from o in output
               select new TaskItem(o.ToString());
      
      var membersToGet = Capture.Split(',').Select(s => s.Trim(' ')).ToList();

      return
        from o in output
         let matchingMembers = 
           (from m in membersToGet
            let prop = o.Properties.Match(m).FirstOrDefault()
            where prop != null
            select new {prop.Name, prop.Value}).ToList()
         where matchingMembers.Count > 0
         select
           new TaskItem(matchingMembers[0].Value.ToString(),
                        matchingMembers.Skip(1).ToDictionary(a => a.Name, a => a.Value));
    }
  }
}