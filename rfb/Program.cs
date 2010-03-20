using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace rfb
{
  class Program
  {
    //[STAThread]
    static void Main(string[] args)
    {
      AppDomain.CurrentDomain.AssemblyResolve += onAssemblyResolve;
      AppDomain.CurrentDomain.TypeResolve += onTypeResolve;
      //Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

      var t = new Thread(runRfb);
      t.SetApartmentState(ApartmentState.STA); //OMG! warning MSB4056!
      t.Start(args);
      t.Join();
    }

    private static void runRfb(object arg)
    {
      var args = (string[]) arg;
      var setup = new BuilderSetup();
      var showHelp = false;
      var options =
        new OptionSet
          {
            {"b|build=", "The build script to run", v => setup.BuildFile = v},
            {"o|output=", "Instead of building, write the XML MSBuild script to the specified file", v => setup.OutputXml = v},
            {"t|target=", "The target to execute", v => setup.Target = v},
            {"p|property=", "Properties you want to pass into the script", v => setup.Property = v},
            {"l|logger=", "Fully qualified typename to a logger you want to use. It must implement the Microsoft.Framework.Build.ILogger interface and have a parameterless constructor.", v => setup.LoggerType = v},
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
      catch (Exception x)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Unhandled exception:");
        Console.WriteLine("{0} - {1} at {2}", x.GetType().Name, x.Message, x.StackTrace);
        Console.ResetColor();
      }

      end:
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("rfb done.");
      Console.ResetColor();
    }

    private static Assembly onTypeResolve(object sender, ResolveEventArgs args)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("System is trying to resolve type {0}", args.Name);
      return null;
    }

    private static Assembly onAssemblyResolve(object sender, ResolveEventArgs args)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("System is trying to resolve assembly {0}", args.Name);
      return null;
    }
  }
}
