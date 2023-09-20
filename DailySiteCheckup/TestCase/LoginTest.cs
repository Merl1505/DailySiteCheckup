using DailySiteCheckup.Feature;
using DailySiteCheckup.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.SharePoint.Client;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace DailySiteCheckup.TestCase
{

    public class LoginTest
    {
        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        //  SeleniumTests tests = new SeleniumTests();
        public void MPGSiteLoginTest(IWebDriver driver, Actions builder, List<TestResult> tests,string emailId)
        {
            TestContext.Progress.WriteLine("Execute Login Test.....");
            NavigateToSite navigateToSite = new NavigateToSite();
            navigateToSite.NavigateToHomePage(driver, builder);
            //check if login screen has cookie popup ( close the cookie popup to click on the login button)
            if (ReadFromExcel.IsSignupPageCookiePopup == "Y")
            {
                navigateToSite.ClickAcceptAllCookies(driver, builder);
            }
            EnterCredentials(driver, builder, tests, emailId);
            //ReadFromExcel readFromExcel = new ReadFromExcel();
            //string test = readFromExcel.dataDictionary["1-1"];

        }
        public void EnterCredentials(IWebDriver driver, Actions builder, List<TestResult> tests, string emailId)
        {
            //enter login credentials
            
            driver.FindElement(By.Id("signInName")).SendKeys(emailId);
            driver.FindElement(By.Id("password")).SendKeys(configuration["FirstPassword"]);
            Thread.Sleep(1000);
            
            //click on submit
            IAction submitAction = builder.Click(driver.FindElement(By.Id("next"))).Build();
            submitAction.Perform();
            Thread.Sleep(30000);
            
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
                tests[0].LoginStatus = "Y";
                //tests.Add(new TestResult { SiteURL = ReadFromExcel.SiteURL,SiteName=ReadFromExcel.SiteName, LoginStatus = "Y", Message = "Success", TestCaseName = "Login" });
            }
            else
            {
                TestContext.Progress.WriteLine("Login Failed....");
                tests[0].LoginStatus = "N";
               // tests.Add(new TestResult { SiteURL = ReadFromExcel.SiteURL,SiteName= ReadFromExcel.SiteName, LoginStatus = "N", Message = "Fail", TestCaseName = "Login" });
            }
            //PasswordChange pwdchange = new PasswordChange();
            //pwdchange.ChangePassword(driver, builder);
            Thread.Sleep(2000);
            if(ReadFromExcel.IsConsentPopup == "Y")
            {
                SiteHelper siteHelper = new SiteHelper();
                siteHelper.ConsentPopupComplete(driver, builder);
            }
           
        }

    }
}
