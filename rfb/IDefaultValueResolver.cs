using rfb.Token;

namespace rfb
{
  public interface IDefaultValueResolver
  {
    void Normalize(TokenWithOptions token);
  }
}