using System.ComponentModel.DataAnnotations;

namespace EmailAPI.Models
{
  public class EmailRecord
  {
    [Key]
    public Guid Id { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }

    public EmailRecord(Guid id, string to, string subject, string content, DateTime created)
    {
      Id = id;
      To = to;
      Subject = subject;
      Content = content;
      Created = created;
    }
  }
}
