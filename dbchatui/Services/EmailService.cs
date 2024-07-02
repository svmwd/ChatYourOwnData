using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace YourOwnData.Services
{
    public class EmailService
    {
        public async Task<bool> SendEmailAsync(string emailTo, string emailBody)
        {
            bool success = true;
            try
            {
                SMTP smtp = GlobalValues.GetSMTP();
                using (SmtpClient smtpClient = new SmtpClient(smtp.HostName))
                {
                    smtpClient.Port = 25;
                    smtpClient.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                    smtpClient.EnableSsl = false;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtp.EmailFrom);
                        mailMessage.Subject = "Test Email using Semantic Kernel";
                        mailMessage.Body = emailBody;
                        mailMessage.To.Add(emailTo);

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (SmtpException ex)
            {
                success = false;
                throw new ApplicationException($"SmtpException occurred: {ex.Message}");
            }
            return success;
        }
    }
}


