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
    readonly List<PSScriptToken> scripts = new List<PSScriptToken>();

    public void AddScriptTask(BuildTask task, PSWithReturnValueToken variableToken)
    {
      tasks.Add(new TaskDefinition(task,variableToken));
    }

    public void AddScriptTask(BuildTask task, PSScriptCallToken callToken)
    {
      tasks.Add(new TaskDefinition(task, callToken));
    }

    public void AddScript(PSScriptToken script)
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
      private readonly TokenWithOptions scriptUseToken;

      public TaskDefinition(BuildTask task, PSWithReturnValueToken varToken)
      {
        this.task = task;
        scriptUseToken = asNodeWithOptions(varToken);
        task.SetParameterValue("Capture", scriptUseToken["Capture"]);

        var kvSerializer = (KeyValueSerializer)scriptUseToken.Where(kv => kv.Key != "Capture")
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        if (!string.IsNullOrEmpty(kvSerializer.ToString()))
          task.SetParameterValue("CommandParameters", kvSerializer.ToString());

        task.SetParameterValue("ReturnValueType", varToken.ValueType.ToString());
        if (varToken.ValueType.Equals(PSScriptReturnValueType.ItemGroup))
          task.AddOutputItem("ScriptItemOutput", varToken.VariableName);
        if (varToken.ValueType.Equals(PSScriptReturnValueType.Property))
          task.AddOutputProperty("ScriptPropOutput", varToken.VariableName);
      }

      public TaskDefinition(BuildTask task, TokenWithOptions callToken)
      {
        this.task = task;
        scriptUseToken = callToken;
        task.SetParameterValue("Capture", callToken["Capture"]);
        task.SetParameterValue("ReturnValueType", PSScriptReturnValueType.Undefined.ToString());
      }

      public string ScriptName
      {
        get { return scriptUseToken.Word; }
      }

      public void SetScript(string script)
      {
        task.SetParameterValue("Script", script);
      }

      private static AnyToken asNodeWithOptions(AbstractDefinedValueToken varToken)
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