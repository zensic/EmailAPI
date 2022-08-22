using System.ComponentModel.DataAnnotations;

namespace EmailAPI.TokenAuth
{
  public class Token
  {
    [Key]
    public string Value { get; set; }
    public DateTime ExpiryDate { get; set; }
  }
}
