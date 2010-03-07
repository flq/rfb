namespace rfb.Token
{
  public abstract class AbstractToken : IToken
  {
    protected abstract AbstractToken handle(TokenizerHandle handle);
    protected abstract void reset();
    public abstract IToken Clone();
    
    public IToken Check(TokenizerHandle handle)
    {
      var t = this.handle(handle);
      if (t!= null)
      {
        var returnToken = t.Clone();
        t.reset();
        return returnToken;
      }
      return null;
    }

    public abstract void Accept(ITokenStreamVisitor visitor);
  }
}