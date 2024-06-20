using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Diagnostics;
using YourOwnData.Services;

namespace YourOwnData.Plugins
{
    public class WebLaunchPlugin
    {
        [KernelFunction]
        [Description("To launch a url to browser")]
        public string DoLaunch(
        Kernel kernel,
        [Description("web url to launch. default website is our starvision website at: www.starvisionit.com")] string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            return "Launch successfully!";
        }
    }
}
