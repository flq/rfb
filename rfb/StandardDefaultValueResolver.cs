using System;
using System.Collections.Generic;
using rfb.Token;

namespace rfb
{
  public class StandardDefaultValueResolver : IDefaultValueResolver
  {
    private readonly Dictionary<Type, string> knownTokenDictionary =
      new Dictionary<Type, string>
        {
          {typeof(ProjectToken), "DefaultTargets"},
          {typeof(TargetToken), "Name"},
          {typeof(ImportProjectToken), "Project"}
        };
    private readonly Dictionary<string, string> unknownTokenDictionary = 
      new Dictionary<string, string>
        {
          {"Message", "Text"},
          {"RemoveDir", "Directories"},
          {"MakeDir", "Directories"},
          {"Delete", "Files"},
          {"MsBuild", "Projects"},
          {"Exec", "Command"}
        };

    public void Normalize(TokenWithOptions token)
    {
      string @default;
      knownTokenDictionary.TryGetValue(token.GetType(), out @default);
      if (@default == null)
        unknownTokenDictionary.TryGetValue(token.Word, out @default);
      if (@default == null && token["Default"] != null)
        throw new ArgumentException("Cannot map the default you set to any option of token " + token.Word);
      token.MapDefaultTo(@default);
    }
  }
}