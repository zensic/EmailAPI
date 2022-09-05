using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
  public class Message
  {
    public List<MailboxAddress> To { get; set; }
    public List<MailboxAddress> Cc { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public Message(IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
    {
      To = new List<MailboxAddress>();

      To.AddRange(to.Select(x => new MailboxAddress("email API", x)));
      Cc.AddRange(cc.Select(x => new MailboxAddress("email API", x)));
      Subject = subject;
      Content = content;
    }
  }
}
