using ServiceRequestManager.Interfaces;
using System.Net.Mail;

namespace ServiceRequestManager.EmailService
{
	public class EmailClient : IEmailClient
    {
        private readonly IConfiguration _configuration;
		private ILogger<EmailClient> _logger;

		public EmailClient(IConfiguration configuration, ILogger<EmailClient> logger) 
        {
            _configuration = configuration;
            _logger = logger;
        }
		public async Task SendEmail(string Message)
		{
            try
            {
                _logger.LogInformation("Send Email Process Started");
				MailMessage newMail = new MailMessage();
                SmtpClient client = new SmtpClient(_configuration["SMTP_HOST"]);
                newMail.From = new MailAddress(_configuration["SMTP_FROM"], _configuration["SMTP_USER_NAME"]);
                newMail.To.Add(_configuration["SMTP_TO"]);
                newMail.Subject = "Service Request Management Status Notification"; 
                newMail.IsBodyHtml = true;                
                newMail.Body = $"<h1> {Message} </h1>";
                client.EnableSsl = true;
                client.Port = Convert.ToInt32(_configuration["SMTP_PORT"]);
                client.Credentials = new System.Net.NetworkCredential(_configuration["SMTP_USER"], _configuration["SMTP_PASSWORD"]);
                await client.SendMailAsync(newMail);
				_logger.LogInformation("Send Email Process Completed");
			}
			catch (Exception ex)
            {
				_logger.LogInformation($"Send Email Process Some Error Occured {ex.Message} ");
                //Can throw exception if error email has to block Workflow 
            }
        }
	}
}
