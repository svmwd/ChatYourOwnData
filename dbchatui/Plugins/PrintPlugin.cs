using Microsoft.SemanticKernel;
using System.ComponentModel;
using YourOwnData.Services;
using System.Drawing.Printing;
using static System.Net.Mime.MediaTypeNames;

namespace YourOwnData.Plugins
{
    public class PrintPlugin
    {
        [KernelFunction]
        [Description("To print document to a printer.")]
        public string DoPrinting(
        Kernel kernel,
        [Description("Print result to network printer")] string result)
        {
            // Add logic to send an email using the recipientEmails, subject, and body
            PrintService service = new PrintService();
            service.PrintToIPAddress("10.155.203.122", result);
            return "Printed successfully!";
            //Console.WriteLine("Printed " + subject + " successfully!");
        }

        /*
        public void PrintDocument()
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            // Example content to print
            string textToPrint = "Hello, this is a test print!";
            Font printFont = new Font("Arial", 12);
            ev.Graphics.DrawString(textToPrint, printFont, Brushes.Black, 10, 10);
        }

        */
    }
}
