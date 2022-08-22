using EmailAPI.Models;
using EmailAPI.TokenAuth;
using Microsoft.EntityFrameworkCore;

namespace EmailAPI.DataAccess
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<EmailRecord> Records { get; set; }
    public DbSet<Token> Tokens { get; set; }
  }
}
