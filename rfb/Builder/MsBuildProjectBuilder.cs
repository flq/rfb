using System;
using System.Collections.Generic;
using Microsoft.Build.BuildEngine;
using rfb.Builder;
using rfb.Token;

namespace rfb
{
  public class MsBuildProjectBuilder : ITokenStreamVisitor
  {
    private readonly IDefaultValueResolver valueResolver;

    private BuildPropertyGroup currentPropertyGroup;
    private Target currentTarget;
    private PsScriptJanitor scriptJanitor;

    private PsScriptJanitor ScriptJanitor
    {
      get
      {
        return scriptJanitor ?? (scriptJanitor = new PsScriptJanitor());
      }
    }


    public Project Project { get; private set;}

    public MsBuildProjectBuilder(IDefaultValueResolver valueResolver) : this (valueResolver, null)
    {
    }

    public MsBuildProjectBuilder(IDefaultValueResolver valueResolver, Engine engine)
    {
      this.valueResolver = valueResolver;
      Project = new Project(engine, "3.5");
    }

    public void Visit(ProjectToken token)
    {
      valueResolver.Normalize(token);
      Project.DefaultTargets = token["DefaultTargets"];
    }

    public void Visit(TargetToken token)
    {
      valueResolver.Normalize(token);
      currentTarget = Project.Targets.AddNewTarget(token["Name"]);
      getAndDo(
        token, 
        depends => currentTarget.DependsOnTargets = depends, 
        "DependsOnTargets", "Depends", "Deps");
      getAndDo(
        token,
        condition => currentTarget.Condition = condition,
        "Condition", "If", "When");
      
    }

    public void Visit(UsingTaskToken token)
    {
      currentTarget = null; // Out of any previous target context
      valueResolver.Normalize(token);
      if (token["AssemblyFile"] != null)
        Project.AddNewUsingTaskFromAssemblyFile(token["TaskName"], token["AssemblyFile"]);
    }

    public void Visit(ImportProjectToken token)
    {
      // Need to think some here...
    }

    public void Visit(BacktickToken token)
    {
      if (currentTarget == null)
        throw new InvalidOperationException("Unexpected exec command outside of target scope");

      currentTarget
        .AddNewTask("Exec")
        .SetParameterValue("Command", token.BackTickValue);
    }

    public void Visit(AnyToken token)
    {
      if (currentTarget == null)
        throw new ArgumentOutOfRangeException("token", "rfb strives to recognize all top-level elements but token " + token.Word + " is unknown");

      var task = currentTarget.AddNewTask(token.Word);
      valueResolver.Normalize(token);
      foreach (var option in token)
        task.SetParameterValue(option.Key, option.Value);
    }

    public void Visit(ItemGroupToken token)
    {
      if (currentTarget == null)
      {
        var itemGroup = Project.AddNewItemGroup();
        var values = token.Value.Split(';');  
        foreach (var item in values)
          itemGroup.AddNewItem(token.VariableName, item);
        return;
      }

      //Construct the thingy into the target
      var t = currentTarget.AddNewTask("CreateItem");
      t.SetParameterValue("Include", token.Value);
      t.AddOutputItem("Include", token.VariableName);
    }

    public void Visit(VariableToken token)
    {
      if (currentTarget == null)
      {
        if (currentPropertyGroup == null)
          currentPropertyGroup = Project.AddNewPropertyGroup(false);
        currentPropertyGroup.AddNewProperty(token.VariableName, token.Value);
        return;
      }

      //Construct the thingy into the target
      var t = currentTarget.AddNewTask("CreateProperty");
      t.SetParameterValue("Value", token.Value);
      t.AddOutputProperty("Value", token.VariableName);

    }

    public void Visit(CommentToken token)
    {
      // Nothing to be done
    }

    public void Visit(PsScriptToken token)
    {
      ScriptJanitor.AddScript(token);
    }

    public void Visit(PSWithReturnValueToken token)
    {
      if (currentTarget == null)
        throw new InvalidOperationException("Unexpected PS value assignment outside of a target");
      ScriptJanitor.AddScriptTask(currentTarget.AddNewTask("RunScript"), token);
    }

    public void Visit(EndToken token)
    {
      ScriptJanitor.MopUp(Project);
    }

    private static void getAndDo(
      TokenWithOptions token, 
      Action<string> returnedValue, 
      params string[] possibleArgumentNames)
    {
      var value = token[possibleArgumentNames];
      if (value != null)
        returnedValue(value);
    }
  }
}