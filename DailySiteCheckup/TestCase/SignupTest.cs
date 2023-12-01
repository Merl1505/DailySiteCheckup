using DailySiteCheckup.Feature;
using Microsoft.Extensions.Configuration;
using Microsoft.SharePoint.Marketplace.CorporateCuratedGallery;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumNUnitConsoleApp;
using System;
using System.CodeDom.Compiler;

namespace DailySiteCheckup.TestCase
{

    public class SignupTest
    {
       
        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        public void MPGSiteSignupTest(IWebDriver driver, Actions builder, List<TestResult> tests,string emailId)
        {
            bool IsError = false;
            TestContext.Progress.WriteLine("Execute Signup Test.....");
            NavigateToSite navigateToSite = new NavigateToSite();
            navigateToSite.NavigateToHomePage(driver, builder);

            //check if login screen has cookie popup ( close the cookie popup to click on the signup link)
            if (ReadFromExcel.IsSignupPageCookiePopup == "Y")
            {
                navigateToSite.ClickAcceptAllCookies(driver, builder);
            }

            //click on signup link
            IAction signuplinkClick = builder.Click(driver.FindElement(By.Id("createAccount"))).Build();
            signuplinkClick.Perform();
            Thread.Sleep(6000);
           
            EmailVerification(driver,builder,emailId);
            // enter other fileds
            ProcessSignupFields(driver,builder);
            //driver.FindElement(By.Id("newPassword")).SendKeys(configuration["FirstPassword"]);
            //driver.FindElement(By.Id("givenName")).SendKeys(configuration["FirstName"]);
            //driver.FindElement(By.Id("surname")).SendKeys(configuration["SecondName"]);

            //// click on check box and radio buttons
            //IAction actionchkbox = builder.Click(driver.FindElement(By.Id("extension_TermsOfUseConsented_True"))).Build();
            //actionchkbox.Perform();

            //click on submit
            IAction submitAction = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            submitAction.Perform();
            Thread.Sleep(10000);
            //find if error has occured 
            IWebElement labelerror = (IWebElement)driver.FindElement(By.Id("claimVerificationServerError"));
            IsError = labelerror.Text.Contains("incorrect.");
           //bool account_created_txt;
            // get test result
            if (!IsError)
            {
                //IWebElement labelAccountCreated = (IWebElement)driver.FindElement(By.XPath("//div[@class = 'attrEntry']/label[@for = 'successhdg']"));
                //account_created_txt = labelAccountCreated.Text.Contains("Your account has been created");
                //Assert.IsTrue(account_created_txt);
                string currentUrl = driver.Url;
                Assert.IsTrue(currentUrl.Contains("signup_signin") || currentUrl.Contains("SIGNUP_SIGNIN"));

                if (currentUrl.Contains("signup_signin") || currentUrl.Contains("SIGNUP_SIGNIN"))
                {
                    TestContext.Progress.WriteLine("Signup Success.....Email Id is :"+ emailId);
                    tests.Add(new TestResult { SiteURL = ReadFromExcel.SiteURL, SiteName = ReadFromExcel.SiteName, SignUpStatus = "Y", Message = "Success", TestCaseName = "Signup" });
                }
               
            }

            else
            {
                TestContext.Progress.WriteLine("Signup Failed.....Incorrect username and password combinations");
                tests.Add(new TestResult { SiteURL = ReadFromExcel.SiteURL, SiteName = ReadFromExcel.SiteName, SignUpStatus = "N", Message = "Fail", TestCaseName = "Signup" });
            }


        }
        public void ProcessSignupFields(IWebDriver driver, Actions builder)
        {
            // check if the registration page is having a cookiepopup
            NavigateToSite navigateToSite = new NavigateToSite();
           
            if (ReadFromExcel.IsRegistrationPageCookiePopup == "Y")
            {
                navigateToSite.ClickAcceptAllCookies(driver, builder);
            }
            string url; 
            Dictionary<string, Dictionary<string, string>> SignupDetailsDict = ReadFromExcel.SignupDetailsDic;
            foreach (var kvp in SignupDetailsDict)
            {
                url = kvp.Key;
                foreach (var innerKvp in kvp.Value)
                {
                    if (innerKvp.Key == "New_Password" && innerKvp.Value != "null")
                    {
                        driver.FindElement(By.Id(innerKvp.Value)).SendKeys(configuration["FirstPassword"]);
                    }
                    if (innerKvp.Key == "Given_Name" && innerKvp.Value != "null")
                        driver.FindElement(By.Id(innerKvp.Value)).SendKeys(configuration["FirstName"]);
                    if (innerKvp.Key == "Surname" && innerKvp.Value != "null")
                        driver.FindElement(By.Id(innerKvp.Value)).SendKeys(configuration["SecondName"]);
                    if (innerKvp.Key == "Mobile_Num" && innerKvp.Value != "null")
                        driver.FindElement(By.Id(innerKvp.Value)).SendKeys(configuration["mobile"]);
                    if (innerKvp.Key == "Middle_Name" && innerKvp.Value != "null")
                        driver.FindElement(By.Id(innerKvp.Value)).SendKeys(configuration["middle_name"]);
                    if (innerKvp.Key == "NIE" && innerKvp.Value != "null")
                    {
                        
                        string NIE_val = GenerateNIECombinations();
                        driver.FindElement(By.Id(innerKvp.Value)).SendKeys(NIE_val);
                    }
                    
                    // click on check box and radio buttons
                    IAction actionchkbox; IWebElement radioButton;
                    if (innerKvp.Key == "FirstConsent" && innerKvp.Value != "null")
                    {
                        Thread.Sleep(1000);
                        actionchkbox = builder.Click(driver.FindElement(By.Id(innerKvp.Value))).Build();
                        actionchkbox.Perform();
                    }
                    if (innerKvp.Key == "SecondConsent" && innerKvp.Value != "null")
                    {
                        Thread.Sleep(1000);
                        actionchkbox = builder.Click(driver.FindElement(By.Id(innerKvp.Value))).Build();
                        actionchkbox.Perform();
                    }
                    if (innerKvp.Key == "ThirdConsent" && innerKvp.Value != "null")
                    {
                        Thread.Sleep(1000);
                        actionchkbox = builder.Click(driver.FindElement(By.Id(innerKvp.Value))).Build();
                        actionchkbox.Perform();
                    }
                    if (innerKvp.Key == "RadioButton1" && innerKvp.Value != "null")
                    {
                        //radioButton = driver.FindElement(By.Id(innerKvp.Value));
                       
                        radioButton = driver.FindElement(By.CssSelector($"label[for='{innerKvp.Value}']"));
                        string idval = radioButton.GetAttribute("id");
                        radioButton.Click();

                    }
                    if (innerKvp.Key == "RadioButton2" && innerKvp.Value != "null")
                    {
                        radioButton = driver.FindElement(By.CssSelector($"label[for='{innerKvp.Value}']"));
                        string idval = radioButton.GetAttribute("id");
                        radioButton.Click();
                    }
                    if (innerKvp.Key == "RadioButton3" && innerKvp.Value != "null")
                    {
                        radioButton = driver.FindElement(By.CssSelector($"label[for='{innerKvp.Value}']"));
                        string idval = radioButton.GetAttribute("id");
                        radioButton.Click();
                    }
                    //ReadFromExcel.SignupDetailsDic.Remove(url);
                   // break;
                }
                break;
            }
        }
        public void EmailVerification(IWebDriver driver, Actions builder, string emailId)
        {
            Thread.Sleep(1000); 
            //enter email
            string tempEmailId = configuration["TempMailId"];
            driver.FindElement(By.Id("email")).SendKeys(emailId);

            //click send verification code 
            IAction SendEmailVerification_action = builder.Click(driver.FindElement(By.Id("emailVerificationControl_but_send_code"))).Build();
            SendEmailVerification_action.Perform();
            Thread.Sleep(3500);

            // enter otp (temporarily ask for otp to enter manually)
            TestContext.Progress.WriteLine("Enter the Email Verification Code for SignupTest.....");
            //string Signup_EmailVerificationCode = Console.ReadLine();

            //get the otp through mailsac API end points
            ReadEmailForOtp readEmail = new ReadEmailForOtp();
            var task = readEmail.ReadMailsacEmailAPIAsync(emailId);
            var OTP_input = task.Result;
            string Signup_EmailVerificationCode = OTP_input.Substring(0, 4);
            TestContext.Progress.WriteLine("Email OTP is..." + Signup_EmailVerificationCode);
            Thread.Sleep(1000);
            driver.FindElement(By.Id("VerificationCode")).SendKeys(Signup_EmailVerificationCode);

            By elementLocator = By.Id("emailVerificationControl_but_verify_code"); // Check if verify code button is present after otp is entered
            IWebElement element = driver.FindElement(elementLocator);
            string styleAttributeValue = element.GetAttribute("style");
            bool isDisplayNone = styleAttributeValue.Contains("display: none");
            if (!isDisplayNone)
            {
                IAction verifycode_action = builder.Click(driver.FindElement(By.Id("emailVerificationControl_but_verify_code"))).Build();
                verifycode_action.Perform();
            }
            Thread.Sleep(3500);
        }

        public string GenerateNIECombinations()
        {
            string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] result = new char[6];

            for (int i = 0; i < 6; i++)
            {
                int index = random.Next(validCharacters.Length);
                result[i] = validCharacters[index];
            }

            string randomString = new string(result);
            return randomString;
        }
    }
}
