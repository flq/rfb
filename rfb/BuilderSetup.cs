using System;

namespace rfb
{
  public class BuilderSetup
  {
    public string BuildFile { get; set; }

    public void Validate()
    {
      if (BuildFile == null)
        throw new ValidationException("No Build file was specified");
    }
  }
}