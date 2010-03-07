namespace rfb.Token
{
  public interface IToken
  {
    IToken Check(TokenizerHandle handle);
    void Accept(ITokenStreamVisitor visitor);
  }
}