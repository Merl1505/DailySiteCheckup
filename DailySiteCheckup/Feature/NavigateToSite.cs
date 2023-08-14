using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumNUnitConsoleApp;

namespace DailySiteCheckup.Feature
{
    public class NavigateToSite
    {
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
                    Console.WriteLine($"Header: {innerKvp.Key}, Value: {innerKvp.Value}");
                    // check if cookies popup appear
                    if (innerKvp.Key == "Cookie_Popup" && innerKvp.Value == "Y")
                    {
                        IWebElement CookiePopupelement = driver.FindElement(By.Id("onetrust-group-container"));
                        string CookiePopupAttr = CookiePopupelement.GetAttribute("id");
                        if (CookiePopupAttr == "onetrust-group-container")
                        {
                            IAction cookie_accept_action = builder.Click(driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Build();
                            cookie_accept_action.Perform();
                            Thread.Sleep(1000);

                        }
                        break;
                    }
                }
                Console.WriteLine($"Key: {kvp.Key}");
                ReadFromExcel.SiteDetailsDic.Remove(url);
                break;
            }
            
            //Click action - signup link click
            IAction user_iconclick_action = builder.Click(driver.FindElement(By.ClassName("user-icon"))).Build();
            user_iconclick_action.Perform();
            Thread.Sleep(8000);

        }
    }
}
