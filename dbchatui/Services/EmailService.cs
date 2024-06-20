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
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("mwdwebuat@gmail.com", "MWD#1uat");
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("mwdwebuat@gmail.com");
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


