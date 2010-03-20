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

      using (var psRunner = PowershellRunspace.CreateRunner(Log))
      {
        var output = psRunner.InvokeScript(Script, @params);

        var retValType = (PSScriptReturnValueType) Enum.Parse(typeof (PSScriptReturnValueType), ReturnValueType);

        if (retValType.Equals(PSScriptReturnValueType.ItemGroup))
          ScriptItemOutput = convertOutput(output).ToArray();
        else if (retValType.Equals(PSScriptReturnValueType.Property))
          ScriptPropOutput = convertOutput(output).Last().ItemSpec;
      }
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
            let prop = matchMember(o, m)
            where prop != null
            select new {prop.Name, prop.Value}).ToList()
         where matchingMembers.Count > 0
         select
           new TaskItem(matchingMembers[0].Value.ToString(),
                        matchingMembers.ToDictionary(a => a.Name, a => a.Value));
    }

    private static Info matchMember(PSObject psObject, string member)
    {
      var property = psObject.Properties.Match(member).FirstOrDefault();
      if (property != null)
        return new Info {Name = property.Name, Value = property.Value};

      //Try if captured item is a hashtable - then we do the same as powershell does, mapping the capture to a key
      if (psObject.BaseObject is System.Collections.Hashtable)
      {
        var table = (System.Collections.Hashtable) psObject.BaseObject;
        if (table.ContainsKey(member))
          return new Info { Name = member, Value = table[member].ToString()}; //MSBuild only understands strings
      }
      return null;
    }

    private class Info
    {
      public string Name;
      public object Value;
    }
  }
}