using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mailing;

public class EmailService : IEmailService
{
  private readonly EmailSettings _emailSettings;
  private readonly ILogger<EmailService> _logger;

  public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
  {
    _emailSettings = options.Value;
    _logger = logger;
  }

  public async Task<bool> SendEmailAsync(Email email)
  {
    var client = new SendGridClient(_emailSettings.ApiKey);

    var subject = email.Subject;
    var to = new EmailAddress(email.To);
    var emailBody = email.Body;

    var from = new EmailAddress
    {
      Email = _emailSettings.FromAddress,
      Name = _emailSettings.FromName,
    };

    var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);

    var response = await client.SendEmailAsync(sendGridMessage);


    if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
    {
      _logger.LogInformation("Email sent.");
      return true;
    }

    _logger.LogError("Email failed.");
    return false;
  }
}