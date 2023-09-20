using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumNUnitConsoleApp;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.DevTools.V112.Debugger;
using NUnit.Framework;

namespace DailySiteCheckup.Feature
{
    public class NavigateToSite
    {
        private string siteName;
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
               
                url = kvp.Key;// redirecting to a url
                driver.Navigate().GoToUrl(url);
                driver.Manage().Window.Maximize();
                Thread.Sleep(10000);
                
                Console.WriteLine();
                foreach (var innerKvp in kvp.Value)
                {
                    if (innerKvp.Key == "Site_Name")
                        siteName = innerKvp.Value;
                    if (innerKvp.Key == "Signup_page_cookie_popup")
                        IsSignupPageCookiePopup = innerKvp.Value;
                    if (innerKvp.Key == "consent_popup_modal")
                        IsConsentPopup = innerKvp.Value;
                    if (innerKvp.Key == "consent_popup_checkbox1")
                        ConsentPopupChkBx1 = innerKvp.Value;
                    if (innerKvp.Key == "consent_popup_checkbox2")
                        ConsentPopupChkBx2 = innerKvp.Value;    

                    // check if cookies popup appear
                    if (innerKvp.Key == "Cookie_Popup" && innerKvp.Value == "Y")
                    {
                        Thread.Sleep(7000);
                        ClickAcceptAllCookies(driver, builder);
                        break;
                    }
                }
                ReadFromExcel.SiteURL = url;
                ReadFromExcel.SiteName = siteName;
                ReadFromExcel.IsSignupPageCookiePopup = IsSignupPageCookiePopup;
                ReadFromExcel.IsConsentPopup = IsConsentPopup;
                ReadFromExcel.ConsentPopupChkBx1 = ConsentPopupChkBx1;
                ReadFromExcel.ConsentPopupChkBx2 = ConsentPopupChkBx2;
                break;
            }
            
            //Click action - signup link click
            IAction user_iconclick_action = builder.Click(driver.FindElement(By.ClassName("user-icon"))).Build();
            user_iconclick_action.Perform();
            Thread.Sleep(10000);

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
                TestContext.Progress.WriteLine("No cookie popup at signup screen....");

        }
    }
}
