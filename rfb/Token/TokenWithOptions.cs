using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace rfb.Token
{
  public abstract class TokenWithOptions : AbstractToken, IEnumerable<KeyValuePair<string,string>>
  {
    protected readonly static Regex startWord = new Regex(@"^\s*(\w+?)\s", RegexOptions.Compiled);
    private readonly static Regex defaultOption = new Regex(@"""(.+)""", RegexOptions.Compiled);
    private readonly static Regex optionFinder = new Regex(@"-(\w+?):(.+?)(?=-|$)", RegexOptions.Compiled);

    protected TokenWithOptions()
    {
      Options = new Dictionary<string, string>();
    }

    public Dictionary<string, string> Options { get; private set; }
    public string Word { get; private set; }

    protected abstract Regex matchCondition { get; }

    protected override AbstractToken handle(TokenizerHandle handle)
    {
      if (handle.CurrentLineMatches(matchCondition))
      {
        completeTokenization(handle);
        return this;
      }
      return null;
    }

    protected void completeTokenization(TokenizerHandle tHandle)
    {
      extractMainWord(tHandle);
      extractDefaultOption(tHandle);
      int processedMatches;
      var handlingTheFirstLine = true;
      do
      {
        processedMatches = extractOptions(tHandle, handlingTheFirstLine);
        if (processedMatches > 0 || handlingTheFirstLine)
        {
          tHandle.CurrentHandled();
          processedMatches = 1;
          handlingTheFirstLine = false;
        }
        tHandle.Advance();
      } while (processedMatches > 0);
      
    }

    private void extractDefaultOption(TokenizerHandle tHandle)
    {
      var m = defaultOption.Match(tHandle.CurrentLine);
      if (m.Groups.Count == 2)
        Options.Add("Default", m.Groups[1].Value);
    }

    private void extractMainWord(TokenizerHandle tHandle)
    {
      var m = startWord.Match(tHandle.CurrentLine);
      if (m.Groups.Count < 2)
        throw new ArgumentException("Could not find a node with option word in the current line");
      Word = m.Groups[1].Captures[0].Value;
    }

    private int extractOptions(TokenizerHandle tHandle, bool isAStartingWordOK)
    {
      if (tHandle.CurrentLine == null)
        return 0;

      if (!isAStartingWordOK && startWord.IsMatch(tHandle.CurrentLine))
        return 0;

      var matches = optionFinder.Matches(tHandle.CurrentLine);
      foreach (Match m in matches)
      {
        if (m.Groups.Count != 3 && m.Groups.Count != 0)
          throw new ArgumentException("Syntax error while trying to parse options.");
        Options.Add(m.Groups[1].Value, m.Groups[2].Value.TrimEnd(' '));
      }
      return matches.Count;
    }

    protected override void reset()
    {
      Options.Clear();
      Word = null;
    }

    public override IToken Clone()
    {
      var clone = (TokenWithOptions) MemberwiseClone();
      clone.Options = new Dictionary<string, string>(Options);
      return clone;
    }

    public string this[string argumentName]
    {
      get
      {
        string @return;
        Options.TryGetValue(argumentName, out @return);
        return @return;
      }
    }

    public string this[string[] argumentNames]
    {
      get
      {
        foreach (var arg in argumentNames)
        {
          string @return;
          Options.TryGetValue(arg, out @return);
          if (@return != null) return @return;
        }
        return null;
      }
    }

    public void MapDefaultTo(string defaultOption)
    {
      var defaultOptionValue = this["Default"];
      if (defaultOptionValue == null) return;
      Options.Remove("Default");
      Options[defaultOption] = defaultOptionValue;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      return Options.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}