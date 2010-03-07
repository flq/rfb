using System.IO;
using rfb.Token;

namespace rfb.Tests.Utils
{
  public static class DataMother
  {
    public static void FastForwardToLine(this TokenizerHandle handle, int line)
    {
      for (var i = 0; i < line - 1; i++)
      {
        handle.Advance();
        handle.CurrentHandled();
      }
      handle.Advance();
    }

    public static Tokenizer Tokenized(this string fileName)
    {
      var file = GetFile(fileName);
      return new Tokenizer(new StringReader(file));
    }

    public static T ProcessWithToken<T>(this TokenizerHandle h) where T : IToken, new()
    {
      return (T)new T().Check(h);
    }

    public static string GetEmptyProjectFile()
    {
      return loadFile("emptyproject.txt");
    }

    public static string GetNodeWithOptions()
    {
      return loadFile("node with numerous options.txt");
    }

    public static string ProjectAndTarget()
    {
      return loadFile("project and target.txt");
    }

    public static string NodeWithOptionsVariantB()
    {
      return loadFile("node w options b.txt");
    }

    public static string GetFile(string file)
    {
      return loadFile(file);
    }

    private static string loadFile(string name)
    {
      var stream = typeof(DataMother).Assembly
        .GetManifestResourceStream(typeof(DataMother), name);
      var sr = new StreamReader(stream);

      return sr.ReadToEnd();
    }
  }
}