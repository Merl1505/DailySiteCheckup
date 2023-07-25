﻿using DailySiteCheckup.Feature;
using DailySiteCheckup.Feature;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumNUnitConsoleApp;

namespace DailySiteCheckup.TestCase
{

    public class LoginTest
    {
        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        //  SeleniumTests tests = new SeleniumTests();
        public void MPGSiteLoginTest(IWebDriver driver, Actions builder, List<TestResult> tests)
        {
            TestContext.Progress.WriteLine("Execute Login Test.....");
            NavigateToSite navigateToSite = new NavigateToSite();
            navigateToSite.NavigateToHomePage(driver, builder);
            EnterCredentials(driver, builder, tests);
            //ReadFromExcel readFromExcel = new ReadFromExcel();
            //string test = readFromExcel.dataDictionary["1-1"];

        }
        public void EnterCredentials(IWebDriver driver, Actions builder, List<TestResult> tests)
        {
            //enter login credentials

            driver.FindElement(By.Id("signInName")).SendKeys(configuration["TempMailId"]);
            driver.FindElement(By.Id("password")).SendKeys(configuration["FirstPassword"]);
            Thread.Sleep(1000);

            //click on submit
            IAction submitAction = builder.Click(driver.FindElement(By.Id("next"))).Build();
            submitAction.Perform();
            Thread.Sleep(15000);

            // get test result
            IWebElement element = driver.FindElement(By.ClassName("login"));
            // Get the value of the "class" attribute
            string classAttributeValue = element.GetAttribute("class");
            // Check if the class attribute value contains the desired class
            bool hasDesiredClass = classAttributeValue.Contains("logged-in");
            Assert.IsTrue(classAttributeValue.Contains("logged-in"));
            if (hasDesiredClass)
            {
                TestContext.Progress.WriteLine("Login Success....");
                tests.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "Y", Message = "Success", TestCaseName = "Login" });
            }
            else
            {
                TestContext.Progress.WriteLine("Login Failed....");
                tests.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "N", Message = "Fail", TestCaseName = "Login" });
            }
            //PasswordChange pwdchange = new PasswordChange();
            //pwdchange.ChangePassword(driver, builder);

        }

    }
}
