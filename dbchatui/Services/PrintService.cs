using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace YourOwnData.Services
{
    public class PrintService
    {
        public void PrintToIPAddress(string ipAddress, string strToPrint)
        {
            try
            {
                int port = 9100;

                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(ipAddress, port);
                //StreamReader reader = new StreamReader(txtFilename.Text.ToString()); //ie: C:\\Apps\\test.txt
                StreamWriter writer = new StreamWriter(client.GetStream());
                //string testFile = reader.ReadToEnd();
                //reader.Close();
                writer.WriteLine(strToPrint);
                writer.Flush();
                writer.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Error");
                Console.WriteLine(ex);
            }
        }

    }
}
