using System;
using System.Collections.Generic;
using rfb.Token;

namespace rfb.Tests
{
  public class AssertingTokenStreamVisitor : ITokenStreamVisitor
  {
    public List<IToken> Tokens = new List<IToken>();

    public int TotalCount;
    public int VisitedProjectToken;
    public int VisitedTargetToken;
    public int VisitedAnyToken;
    public int VisitedItemGroupToken;
    public int VisitedVariableToken;
    public int VisitedCommentToken;
    public int VisitedUsingTaskToken;
    public int VisitedImportProjectToken;
    public int VisitedBacktick;

    public void Visit(ProjectToken token)
    {
      VisitedProjectToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(TargetToken token)
    {
      VisitedTargetToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(UsingTaskToken token)
    {
      VisitedUsingTaskToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(ImportProjectToken token)
    {
      VisitedImportProjectToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(BacktickToken token)
    {
      VisitedBacktick++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(AnyToken token)
    {
      VisitedAnyToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(ItemGroupToken token)
    {
      VisitedItemGroupToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(VariableToken token)
    {
      VisitedVariableToken++;
      TotalCount++;
      Tokens.Add(token);
    }

    public void Visit(CommentToken token)
    {
      VisitedCommentToken++;
      TotalCount++;
      Tokens.Add(token);
    }
  }
}