using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace DailySiteCheckup.Feature
{
    public class NavigateToSite
    {
        private string siteName;
        private string IsLoginPageCookiePopup;
        private string IsSignupPageCookiePopup;
        private string IsConsentPopup;
        public  string ConsentPopupChkBx1;
        public string ConsentPopupChkBx2;

        public void NavigateToHomePage(IWebDriver driver, Actions builder)
        {
            string url;
            
            Dictionary<string, Dictionary<string, string>> SiteDetailsDictionary = ReadFromExcel.SiteDetailsDic;
            //Process values in the nested dictionary
            foreach (var kvp in SiteDetailsDictionary)
            {
               
                url = kvp.Key;// redirecting to a home page of the website
                driver.Navigate().GoToUrl(url);
                driver.Manage().Window.Maximize();
                Thread.Sleep(10000);
                
                Console.WriteLine();
                foreach (var innerKvp in kvp.Value)
                {
                    if (innerKvp.Key == "Site_Name")
                        siteName = innerKvp.Value;
                    if (innerKvp.Key == "Login_page_cookie_popup")
                        IsLoginPageCookiePopup = innerKvp.Value;
                    if (innerKvp.Key == "Signup_page_cookie_popup")
                        IsSignupPageCookiePopup = innerKvp.Value;
                    if (innerKvp.Key == "consent_popup_modal")
                        IsConsentPopup = innerKvp.Value;
                    if (innerKvp.Key == "consent_popup_checkbox1")
                        ConsentPopupChkBx1 = innerKvp.Value;
                    if (innerKvp.Key == "consent_popup_checkbox2")
                        ConsentPopupChkBx2 = innerKvp.Value;

                    // check if cookies popup appear
                    if (innerKvp.Key == "Home_page_cookie_popup" && innerKvp.Value == "Y")
                    {
                        Thread.Sleep(7000);
                        ClickAcceptAllCookies(driver, builder);
                        break;
                    }
                }
                ReadFromExcel.SiteURL = url;
                ReadFromExcel.SiteName = siteName;
                ReadFromExcel.IsLoginPageCookiePopup = IsLoginPageCookiePopup;
                ReadFromExcel.IsSignupPageCookiePopup = IsSignupPageCookiePopup;
                ReadFromExcel.IsConsentPopup = IsConsentPopup;
                ReadFromExcel.ConsentPopupChkBx1 = ConsentPopupChkBx1;
                ReadFromExcel.ConsentPopupChkBx2 = ConsentPopupChkBx2;
                break;
            }
            
            //Click user icon - redirects to login screen
            IAction user_iconclick_action = builder.Click(driver.FindElement(By.ClassName("user-icon"))).Build();
            user_iconclick_action.Perform();
            Thread.Sleep(13000);

        }
        public void ClickAcceptAllCookies(IWebDriver driver, Actions builder)
        {

            IWebElement CookiePopupelement = driver.FindElement(By.Id("onetrust-group-container"));
            if(CookiePopupelement != null)
            {
                string CookiePopupAttr = CookiePopupelement.GetAttribute("id");
                if (CookiePopupAttr == "onetrust-group-container")
                {
                    IAction cookie_accept_action = builder.Click(driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Build();
                    cookie_accept_action.Perform();
                    Thread.Sleep(2000);

                }
            }
            else
                TestContext.Progress.WriteLine("No cookie popup....");

        }
    }
}
