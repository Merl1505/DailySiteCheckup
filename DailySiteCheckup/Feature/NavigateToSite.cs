﻿using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumNUnitConsoleApp;
using OpenQA.Selenium.Support.UI;

namespace DailySiteCheckup.Feature
{
    public class NavigateToSite
    {
        private string siteName;
        private string IsSignupPageCookiePopup;

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
            string CookiePopupAttr = CookiePopupelement.GetAttribute("id");
            if (CookiePopupAttr == "onetrust-group-container")
            {
                IAction cookie_accept_action = builder.Click(driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Build();
                cookie_accept_action.Perform();
                Thread.Sleep(2000);

            }
        }
    }
}
