using System.Reflection;
using OpenQA.Selenium.Interactions;
using System.Xml;
using DailySiteCheckup.Feature;
using DailySiteCheckup.TestCase;
using OpenQA.Selenium;
using NUnit.Framework;
using SeleniumNUnitConsoleApp;
using DailySiteCheckup.Feature;
using Microsoft.Extensions.Configuration;

namespace DailySiteCheckup.TestCase
{
    public class PasswordResetTest
    {
        SeleniumTests tests = new SeleniumTests();
        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        public void MPGSitePasswordReset(IWebDriver driver, Actions builder)
        {
            TestContext.Progress.WriteLine("Execute Forgot Password Test.....");
            NavigateToSite navigateToSite = new NavigateToSite();
            navigateToSite.NavigateToHomePage(driver, builder);

            //Clikc on forgot password link 
            IAction submitAction = builder.Click(driver.FindElement(By.Id("forgotPassword"))).Build();
            submitAction.Perform();
            Thread.Sleep(12000);
            ResetPwd(driver, builder);

        }
        public void ResetPwd(IWebDriver driver, Actions builder)
        {
            string emailId = configuration["TempMailId"];
            driver.FindElement(By.Id("email")).SendKeys(emailId);
            //Click Send Verification Code 
            IAction send_verification_code_action = builder.Click(driver.FindElement(By.Id("emailVerificationControl1_but_send_code"))).Build();
            send_verification_code_action.Perform();
            Thread.Sleep(8000);

            // enter otp (temporarily ask for otp to enter manually)
            TestContext.Progress.WriteLine("Enter the Email Verification Code for Forgot Password Test....");
            //string PwdReset_EmailVerificationCode = Console.ReadLine();

            //get the otp through mailsac API end points
            ReadEmailForOtp readEmail = new ReadEmailForOtp();
            var task = readEmail.ReadMailsacEmailAPIAsync(emailId);
            var PwdReset_EmailVerificationCode = task.Result;
            TestContext.Progress.WriteLine("Email OTP is..." + PwdReset_EmailVerificationCode);
            driver.FindElement(By.Id("VerificationCode")).SendKeys(PwdReset_EmailVerificationCode);
            Thread.Sleep(2000);

            //click on continue
            IAction continue_action = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            continue_action.Perform();
            Thread.Sleep(10000);

            //set new password and confirm password
            driver.FindElement(By.Id("newPassword")).SendKeys(configuration["PasswordReset"]);
            driver.FindElement(By.Id("reenterPassword")).SendKeys(configuration["PasswordReset"]);
            IAction continue_action1 = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            continue_action1.Perform();
            Thread.Sleep(15000);

            /// Get the current URL
            string currentUrl = driver.Url;
            Assert.IsTrue(currentUrl.Contains("signup_signin"));

            if (currentUrl.Contains("signup_signin"))
            {
                TestContext.Progress.WriteLine("Password reset success....");
                tests.testResults.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "Y", Message = "Success", TestCaseName = "Password Reset" });
            }
            else
            {
                TestContext.Progress.WriteLine("Password reset failed....");
                tests.testResults.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "N", Message = "Fail", TestCaseName = "Password Reset" });
            }

        }
    }
}
