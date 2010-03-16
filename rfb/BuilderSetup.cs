using System;
using System.Collections.Generic;
using System.IO;

namespace rfb
{
  public class BuilderSetup
  {
    public string BuildFile { get; set; }
    public string Target { get; set; }
    public string Property { get; set; }
    public string OutputXml { get; set; }
    public string LoggerType { get; set; }

    public Type LoggerAsType
    {
      get
      {
        return Type.GetType(LoggerType);
      }
    }

    public IEnumerable<KeyValuePair<string,string>> Properties
    {
      get
      {
        if (string.IsNullOrEmpty(Property))
          throw new ArgumentNullException("Property");
        var pairs = Property.Split(';');
        foreach (var pair in pairs)
        {
          var tokens = pair.Split('=');
          yield return new KeyValuePair<string, string>(tokens[0], tokens[1]);
        }
      }
    }

    private Stream buildData;
    public Stream BuildData
    {
      get { return buildData ?? File.OpenRead(BuildFile); }
      set { buildData = value; }
    }

    public bool HasProperties
    {
      get { return !string.IsNullOrEmpty(Property); }
    }

    public void Validate()
    {
      if (BuildFile == null && BuildData == null)
        throw new ValidationException("Nothing to build specified");
    }
  }
}