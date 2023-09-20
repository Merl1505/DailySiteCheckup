using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySiteCheckup.Helper
{
    public class SiteHelper
    {
        public void ConsentPopupComplete(IWebDriver driver, Actions builder)
        {
            IWebElement Popupelement = driver.FindElement(By.ClassName("consent_popup_modal"));
            if (Popupelement != null)
            {
                Thread.Sleep(3000);
                IAction actionchkbox;
                actionchkbox = builder.Click(driver.FindElement(By.ClassName("checkmark"))).Build();
                actionchkbox.Perform();

                IAction submitAction = builder.Click(driver.FindElement(By.ClassName("primary-button orange-bg"))).Build();
                submitAction.Perform();
                Thread.Sleep(4000);
            }
        }
    }
}
