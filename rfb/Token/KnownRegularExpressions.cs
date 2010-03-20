using System.Text.RegularExpressions;

namespace rfb.Token
{
  public static class KnownRegularExpressions
  {
    public static readonly Regex CapturingVariableMatch = new Regex(@"[\$@](\w+)\s*<=\s*(.+)$", RegexOptions.Compiled);
    public static readonly Regex ScriptCallMatch = new Regex(@"\s*PS:(\w+)(?!.*<<)", RegexOptions.Compiled);
  }
}