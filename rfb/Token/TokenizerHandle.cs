using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace rfb
{
  [DebuggerDisplay("Handled:{IsCurrentHandled}, Currentline:{CurrentLine}")]
  public class TokenizerHandle
  {
    private bool currentHandled = true;
    public TextReader Reader { get; private set; }
    public string CurrentLine { get; private set; }

    public TokenizerHandle(string contents) : this(new StringReader(contents))
    {
    }

    public TokenizerHandle(TextReader reader)
    {
      Reader = reader;
    }

    public string Advance()
    {
      if (!currentHandled)
        return CurrentLine;

      if (Reader.Peek() != -1)
      {
        CurrentLine = Reader.ReadLine();
        currentHandled = false;
        return CurrentLine;
      }

      CurrentLine = null;
      return null;
    }


    public bool CurrentLineMatches(Regex regex)
    {
      return regex.IsMatch(CurrentLine);
    }

    public void CurrentHandled()
    {
      currentHandled = true;
    }

    public bool IsCurrentHandled
    {
      get { return currentHandled; }
    }
  }
}