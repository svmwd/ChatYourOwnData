using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace YourOwnData.Plugins
{
    public class PrintPlugin
    {
        [KernelFunction]
        [Description("To print document to a printer.")]
        public string DoPrinting(
        Kernel kernel,
        [Description("Print the document to the printer with printername")] string subject,
        string printername
    )
        {
            // Add logic to send an email using the recipientEmails, subject, and body
            // For now, we'll just print out a success message to the console
            return "Printed " + subject + "to " + printername + " successfully!";
            //Console.WriteLine("Printed " + subject + " successfully!");
        }
    }
}
