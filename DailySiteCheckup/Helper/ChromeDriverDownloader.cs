using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
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
    }
}
