using System;
using System.Management.Automation.Host;

namespace rfb.PSSupport
{
  public class PSRawUserItf : PSHostRawUserInterface
  {
    public override KeyInfo ReadKey(ReadKeyOptions options)
    {
      var k = Console.ReadKey();
      return new KeyInfo() { Character = k.KeyChar};
    }

    public override void FlushInputBuffer()
    {
      
    }

    public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
    {
      
    }

    public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
    {
      
    }

    public override BufferCell[,] GetBufferContents(Rectangle rectangle)
    {
      return null;
    }

    public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
    {
      
    }

    private ConsoleColor foregroundColor = ConsoleColor.White;
    public override ConsoleColor ForegroundColor
    {
      get { return foregroundColor; }
      set { foregroundColor = value; }
    }

    private ConsoleColor backgroundColor = ConsoleColor.Black;
    public override ConsoleColor BackgroundColor
    {
      get { return backgroundColor; }
      set { backgroundColor = value; }
    }

    public override Coordinates CursorPosition { get; set; }

    public override Coordinates WindowPosition
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    public override int CursorSize { get; set; }

    public override Size BufferSize { get; set; }

    public override Size WindowSize { get; set; }

    public override Size MaxWindowSize
    {
      get { return new Size(80, 40); }
    }

    public override Size MaxPhysicalWindowSize
    {
      get { return new Size(80, 40); }
    }

    public override bool KeyAvailable
    {
      get { return false; }
    }

    public override string WindowTitle { get; set; }
  }
}