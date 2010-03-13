using rfb.Token;

namespace rfb
{
  public interface ITokenStreamVisitor
  {
    void Visit(ProjectToken token);
    void Visit(TargetToken token);
    void Visit(UsingTaskToken token);
    void Visit(ImportProjectToken token);
    void Visit(BacktickToken token);
    void Visit(AnyToken token);
    void Visit(ItemGroupToken token);
    void Visit(VariableToken token);
    void Visit(CommentToken token);
    void Visit(PsScriptToken token);
    void Visit(PSWithReturnValueToken token);
    void Visit(EndToken token);
  }
}