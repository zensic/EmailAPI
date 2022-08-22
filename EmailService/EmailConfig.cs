namespace EmailService
{
  public class EmailConfig
  {
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public EmailConfig()
    {
      From = "";
      SmtpServer = "";
      Port = 0;
      UserName = "";
      Password = "";
    }

    public EmailConfig(string from, string smtpServer, int port, string userName, string password)
    {
      From = from;
      SmtpServer = smtpServer;
      Port = port;
      UserName = userName;
      Password = password;
    }
  }
}