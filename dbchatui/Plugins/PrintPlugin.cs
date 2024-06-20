using Microsoft.SemanticKernel;
using System.ComponentModel;
using YourOwnData.Services;

namespace YourOwnData.Plugins
{
    public class PrintPlugin
    {
        [KernelFunction]
        [Description("To print document to a printer.")]
        public string DoPrinting(
        Kernel kernel,
        [Description("Print a sentence to network printer")] string sentence)
        {
            // Add logic to send an email using the recipientEmails, subject, and body
            PrintService service = new PrintService();
            service.PrintToIPAddress("10.155.203.122", sentence);
            return "Printed successfully!";
            //Console.WriteLine("Printed " + subject + " successfully!");
        }

    }
}
