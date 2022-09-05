using EmailService;

namespace EmailAPI.Models
{
  public class EmailList
  {
    public IEnumerable<string> To { get; set; }
    public IEnumerable<string> Cc { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public Message ToMessage()
    {
      return new Message(To, Cc, Subject, Content);
    }
  }
}
