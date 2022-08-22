using EmailAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EmailAPI.TokenAuth
{
  public class TokenManager : ITokenManager
  {
    private readonly ApplicationDbContext _context;

    public TokenManager(ApplicationDbContext context)
    {
      _context = context;
    }

    // Generates a new token to be stored in db
    public async Task<Token> NewToken()
    {
      var token = new Token()
      {
        Value = Guid.NewGuid().ToString(),
        ExpiryDate = DateTime.Now.AddMinutes(15),
      };

      await _context.Tokens.AddAsync(token);
      await _context.SaveChangesAsync();

      return token;
    }

    // Check if token exists, return true if exists
    public bool VerifyToken(string token)
    {
      // Check if token exists and has not expired
      //if (_context.Tokens.Any(x => x.Value == token && x.ExpiryDate > DateTime.Now))
      //{
      //  return true;
      //}

      // Check if token exists only
      if (_context.Tokens.Any(x => x.Value == token))
      {
        return true;
      }

      return false;
    }
  }
}
