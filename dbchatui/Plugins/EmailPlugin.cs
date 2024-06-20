using Microsoft.SemanticKernel;
using System.ComponentModel;
using YourOwnData.Services;

namespace YourOwnData.Plugins
{
    public class EmailPlugin
    {
        [KernelFunction]
        [Description("To email message to a recipient email address.")]
        public Task<bool> SendEmail(
      Kernel kernel,
      [Description("Send a message to a recipient email address")] string emailAddress, string emailBody)
        {
            // Add logic to send an email using the recipientEmails, subject, and body
            EmailService service = new EmailService();
            var success = service.SendEmailAsync(emailAddress, emailBody);
            return success;
            //Console.WriteLine("Printed " + subject + " successfully!");
        }
    }
}
