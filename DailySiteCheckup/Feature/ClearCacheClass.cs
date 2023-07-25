using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySiteCheckup.Feature
{
    public class ClearCacheClass
    {
        public void Clearbrowsercache()
        {
            try
            {
                // Clear Internet Explorer cache
                ProcessStartInfo internetExplorerStartInfo = new ProcessStartInfo("cmd.exe", "/c RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
                Process.Start(internetExplorerStartInfo)?.WaitForExit();

                // Clear Mozilla Firefox cache
                ProcessStartInfo firefoxStartInfo = new ProcessStartInfo("cmd.exe", "/c RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 4351");
                Process.Start(firefoxStartInfo)?.WaitForExit();

                // Clear Google Chrome cache
                ProcessStartInfo chromeStartInfo = new ProcessStartInfo("cmd.exe", "/c RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 255");
                Process.Start(chromeStartInfo)?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while clearing the browser cache: " + ex.Message);
            }
        }
    }
}
