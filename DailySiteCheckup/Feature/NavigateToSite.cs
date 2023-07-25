using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySiteCheckup.Feature
{
    public class NavigateToSite
    {
        public void NavigateToHomePage(IWebDriver driver, Actions builder)
        {
            string url = "https://www.experis.com/";
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
            Thread.Sleep(6000);

            //ReadFromExcel readFromExcel = new ReadFromExcel();
            //readFromExcel.GroupSiteandColumns("")

            // check if cookies popup appear
            //IWebElement CookiePopupelement = driver.FindElement(By.Id("onetrust-group-container"));
            //string CookiePopupAttr = CookiePopupelement.GetAttribute("id");
            //if (CookiePopupAttr == "onetrust-group-container")
            //{
            //    IAction cookie_accept_action = builder.Click(driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Build();
            //    cookie_accept_action.Perform();
            //    Thread.Sleep(1000);

            //}
            //Click action - signup link click
            IAction user_iconclick_action = builder.Click(driver.FindElement(By.ClassName("user-icon"))).Build();
            user_iconclick_action.Perform();
            Thread.Sleep(8000);

        }
    }
}
