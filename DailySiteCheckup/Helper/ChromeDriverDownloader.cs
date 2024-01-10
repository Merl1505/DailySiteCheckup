using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DailySiteCheckup.Helper
{
    public class ChromeDriverDownloader
    {
        public static ChromeDriver DownloadChromeDriver(string chromeVersion)
        {
            var chromeDriverUrl = $"https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{chromeVersion}/chromedriver_{chromeVersion}.exe";

            var downloadPath = Path.GetTempFileName() + ".exe";

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(chromeDriverUrl, downloadPath);
            }

            return new ChromeDriver(new ChromeOptions()
            {
                BinaryLocation = downloadPath
            });
        }
        
        public static void ExtractDriver()
        {
            string zipUrl = "https://edgedl.me.gvt1.com/edgedl/chrome/chrome-for-testing/120.0.6099.71/win64/chromedriver-win64.zip";
            string destinationDirectory = "C:\\Users\\Merlin.Savarimuthu\\source\\repos\\Merl1505\\DailySiteCheckup\\DailySiteCheckup\\drivers";

            try
            {
                // Download the ZIP file
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(zipUrl, "temp.zip");
                }

                // Extract the contents of the ZIP file
                ZipFile.ExtractToDirectory("temp.zip", destinationDirectory);

                Console.WriteLine("ZIP file extracted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //finally
            //{
            //    // Clean up by deleting the temporary ZIP file
            //    File.Delete("temp.zip");
            //}
        }
    }
}
