using System;
using System.Collections.Generic;
using System.Linq;

namespace rfb
{
  public class KeyValueSerializer
  {
    private const string pairSeparator = "~+**+~";
    private const string keyValueSeparator = "~*++*~";

    private string serializedString;

    public override string ToString()
    {
      return serializedString;
    }

    public Dictionary<string,string> ToDictionary()
    {
      var result = from kv in serializedString.Split(new [] {pairSeparator}, StringSplitOptions.RemoveEmptyEntries)
                   let pair = kv.Split(new[] { keyValueSeparator }, StringSplitOptions.RemoveEmptyEntries)
                   select new {Key = pair[0], Value = pair[1]};

      return result.ToDictionary(v => v.Key, v => v.Value);
    }

    public static implicit operator KeyValueSerializer(Dictionary<string,string> values)
    {
      return new KeyValueSerializer
               {
                 serializedString =
                   string.Join(pairSeparator, values.Select(v => v.Key + keyValueSeparator + v.Value).ToArray())
               };
    }

    public static implicit operator KeyValueSerializer(string serializedValue)
    {
      return new KeyValueSerializer
      {
        serializedString = serializedValue
      };
    }
  }
}