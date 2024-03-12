using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DailySiteCheckup.Feature
{
    public class VerifyMFA
    {
        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        public void EnterCountryCodeAndPhoneNum(Actions builder,IWebDriver driver)
        {
            IWebElement dropdownElement = driver.FindElement(By.Id("countryCode"));
            SelectElement selectElement = new SelectElement(dropdownElement);
            selectElement.SelectByValue("+91");
            driver.FindElement(By.Id("number")).SendKeys(configuration["mobile"]);

            // click on send verification button if exists
            bool isSendVerificationBtnPresent = driver.FindElements(By.Id("sendCode")).Count > 0;
            if(isSendVerificationBtnPresent)
            {
                IAction submitAction = builder.Click(driver.FindElement(By.Id("sendCode"))).Build();
                submitAction.Perform();
            }
            Thread.Sleep(10000);

            // Put this as a separate function call, as it is also present in account details update cs file

            TestContext.Progress.WriteLine("Enter the OTP sent to the mobile number.....");
            //MessageBox.Show("Please enter the otp sent to your mobile number");

            string phoneNumChange_ver_code = Console.ReadLine();
            driver.FindElement(By.Id("verificationCode")).SendKeys(phoneNumChange_ver_code);
            IAction confirmCode_action = builder.Click(driver.FindElement(By.Id("verifyCode"))).Build();
            confirmCode_action.Perform();
            Thread.Sleep(20000);

        }
    
        public void VerifyMFA_AfterLogin(Actions builder,IWebDriver driver)
        {
            bool isSendVerificationBtnPresent = driver.FindElements(By.Id("sendCode")).Count > 0;
            if (isSendVerificationBtnPresent)
            {
                IAction submitAction = builder.Click(driver.FindElement(By.Id("sendCode"))).Build();
                submitAction.Perform();
            }
            Thread.Sleep(2000);
            TestContext.Progress.WriteLine("Enter the OTP sent to the mobile number.....");
            //MessageBox.Show("Please enter the otp sent to your mobile number");

            string afterlogin_mfa_phone_ver_code = Console.ReadLine();
            driver.FindElement(By.Id("verificationCode")).SendKeys(afterlogin_mfa_phone_ver_code);
            IAction confirmCode_action = builder.Click(driver.FindElement(By.Id("verifyCode"))).Build();
            confirmCode_action.Perform();
            Thread.Sleep(12000);
        }
    }
}
