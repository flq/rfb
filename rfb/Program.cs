using System;
using System.IO;

namespace rfb
{
  class Program
  {
    static void Main(string[] args)
    {
      var setup = new BuilderSetup();
      var showHelp = false;
      var options =
        new OptionSet
          {
            {"b|build=", "The build script to run", v => setup.BuildFile = v},
            {"h|help", "Shows the usage help", v => showHelp = true}
          };
      try
      {
        options.Parse(args);
        if (showHelp)
        {
          options.WriteOptionDescriptions(Console.Out);
          goto end;
        }
        setup.Validate();
        using (var runner = new BuildRunner(setup))
          runner.Run();
      }
      catch (OptionException x)
      {
        Console.WriteLine("The options to rfb were not understood: {0}", x.Message);
        options.WriteOptionDescriptions(Console.Out);
      }
      catch (ValidationException x)
      {
        Console.WriteLine("The options to rfb are not sufficient to get running: {0}", x.Message);
        Console.WriteLine(x.Message);
        options.WriteOptionDescriptions(Console.Out);
      }
      catch (FileNotFoundException x)
      {
        Console.WriteLine("File {0} was not found", x.FileName);
      }
      
      end: 
      Console.WriteLine("rfb done, press any key to finish...");
      Console.ReadLine();
    }
  }
}
