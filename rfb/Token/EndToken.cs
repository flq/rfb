namespace rfb.Token
{
  public class EndToken : IToken
  {
    public IToken Check(TokenizerHandle handle)
    {
      return null;
    }

    public void Accept(ITokenStreamVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}