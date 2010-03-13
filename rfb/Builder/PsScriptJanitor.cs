using System;
using System.Collections.Generic;
using Microsoft.Build.BuildEngine;
using rfb.Token;
using System.Linq;

namespace rfb.Builder
{
  public class PsScriptJanitor
  {
    readonly List<TaskVarCombo> tasks = new List<TaskVarCombo>();
    readonly List<PsScriptToken> scripts = new List<PsScriptToken>();

    public void AddScriptTask(BuildTask task, PSWithReturnValueToken variableToken)
    {
      tasks.Add(new TaskVarCombo(task,variableToken));
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

    private class TaskVarCombo
    {
      private readonly BuildTask task;
      private readonly PSWithReturnValueToken varToken;
      private readonly AnyToken valueOfVar;

      public TaskVarCombo(BuildTask task, PSWithReturnValueToken varToken)
      {
        this.task = task;
        this.varToken = varToken;
        valueOfVar = AsNodeWithOptions;
        if (valueOfVar["Capture"] == null)
          throw new ArgumentNullException("Capture", "The Capture option must be set when running a PS script to return a variable");
        task.SetParameterValue("Capture", valueOfVar["Capture"]);
        if (varToken.ValueType.Equals(PSWithReturnValueToken.ReturnValueType.ItemGroup))
        {
          task.AddOutputItem("ScriptItemOutput", varToken.VariableName);
        }
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