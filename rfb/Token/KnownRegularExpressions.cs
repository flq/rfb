using System.Text.RegularExpressions;

namespace rfb.Token
{
  public static class KnownRegularExpressions
  {
    public static readonly Regex CapturingVariableMatch = new Regex(@"[\$@](\w+)\s*<=\s*(.+)$", RegexOptions.Compiled);
    
  }
}