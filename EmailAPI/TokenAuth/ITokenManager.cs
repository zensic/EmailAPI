namespace EmailAPI.TokenAuth
{
  public interface ITokenManager
  {
    Task<Token> NewToken();
    bool VerifyToken(string token);
  }
}
