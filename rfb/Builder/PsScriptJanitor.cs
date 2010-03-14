using System;
using System.Collections.Generic;
using Microsoft.Build.BuildEngine;
using rfb.Token;
using System.Linq;

namespace rfb.Builder
{
  public class PsScriptJanitor
  {
    readonly List<TaskDefinition> tasks = new List<TaskDefinition>();
    readonly List<PsScriptToken> scripts = new List<PsScriptToken>();

    public void AddScriptTask(BuildTask task, PSWithReturnValueToken variableToken)
    {
      tasks.Add(new TaskDefinition(task,variableToken));
    }

    public void AddScript(PsScriptToken script)
    {
      scripts.Add(script);
    }

    public void MopUp(Project project)
    {
      foreach (var t in tasks)
      {
        var scr = scripts.FirstOrDefault(s => s.ScriptName.Equals(t.ScriptName));
        if (scr == null)
          throw new ArgumentNullException("Access to undefined script " + t.ScriptName);
        t.SetScript(scr.Script);
      }

      if (tasks.Count > 0)
        project.AddNewUsingTaskFromAssemblyName("RunScript", GetType().Assembly.FullName);
    }

    private class TaskDefinition
    {
      private readonly BuildTask task;
      private readonly PSWithReturnValueToken varToken;
      private readonly AnyToken valueOfVar;

      public TaskDefinition(BuildTask task, PSWithReturnValueToken varToken)
      {
        this.task = task;
        this.varToken = varToken;
        valueOfVar = AsNodeWithOptions;
        task.SetParameterValue("Capture", valueOfVar["Capture"]);
        task.SetParameterValue("ReturnValueType", varToken.ValueType.ToString());
        if (varToken.ValueType.Equals(PSScriptReturnValueType.ItemGroup))
          task.AddOutputItem("ScriptItemOutput", varToken.VariableName);
        if (varToken.ValueType.Equals(PSScriptReturnValueType.Property))
          task.AddOutputProperty("ScriptPropOutput", varToken.VariableName);
        
      }

      public string ScriptName
      {
        get { return valueOfVar.Word; }
      }

      public void SetScript(string script)
      {
        task.SetParameterValue("Script", script);
      }

      private AnyToken AsNodeWithOptions
      {
        get
        {
          var h = new TokenizerHandle(varToken.Value);
          h.Advance();
          var anyToken = new AnyToken();
          anyToken = (AnyToken) anyToken.Check(h);
          if (anyToken == null)
            throw new ArgumentException("Value to " + varToken.VariableName + " could not be understood.");
          return anyToken;
        }
      }
    }
  }
}