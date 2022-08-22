using EmailAPI.DataAccess;
using EmailAPI.Filters;
using EmailAPI.Models;
using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace EmailAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [TokenAuthFilter]
  public class MailController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public MailController(ApplicationDbContext context, IEmailSender emailSender)
    {
      _context = context;
      _emailSender = emailSender;
    }

    [HttpPost]
    public async Task<IActionResult> SendMail([FromBody] EmailList emailList)
    {
      if (!ModelState.IsValid)
        return BadRequest("Invalid data.");

      // Transform because dotnet is dumb
      Message tempMessage = emailList.ToMessage();

      // Sends the email
      await _emailSender.SendEmailAsync(tempMessage);

      // Records each email in db, why not?
      foreach (MailboxAddress address in tempMessage.To)
      {
        await _context.Records.AddAsync(new EmailRecord(Guid.NewGuid(), address.ToString(), tempMessage.Subject, tempMessage.Content, DateTime.Now));
        await _context.SaveChangesAsync();
      }

      return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMail()
    {
      // Grab list of all emails
      var emailList = await _context.Records.ToListAsync();

      return Ok(emailList);
    }
  }
}
