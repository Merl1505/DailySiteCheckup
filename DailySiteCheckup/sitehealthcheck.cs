using DailySiteCheckup.Feature;
using DailySiteCheckup.TestCase;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnitLite;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Reflection;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.IO;
using System.Runtime;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using DailySiteCheckup.Helper;
using WebDriverManager.Models.Chrome;
using System.Net.Mail;
using System.Net;
using System.Net.Http;


namespace SeleniumNUnitConsoleApp
{
    [TestFixture]
    public class SeleniumTests
    {
        private IWebDriver driver;
        private Actions builder;
        public static List<TestResult> testResults { get; set; }
       public static string ProfileEditFlag { get; set; }
        public string MailIdToTest { get; set; }
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        public SeleniumTests()
        {
            testResults = new List<TestResult>();
          
            //SiteDetailsDictionary = new Dictionary<string, Dictionary<string, string>>();
        }
        [OneTimeSetUp]
        public void Setup()

        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            ChromeOptions options = new();
            options.AddArgument("--incognito");
            driver = new ChromeDriver(path + @"\drivers\chromedriver-win64\", options);
            builder = new Actions(driver);
        }

        [Test]
        [Order(1)]
        public void SignupTest()
        {
            SignupTest signupTest = new SignupTest();
            CreateTestingEmail createTestingEmail = new CreateTestingEmail();
            MailIdToTest = createTestingEmail.GenerateTestingEmail();
            signupTest.MPGSiteSignupTest(driver, builder, testResults, MailIdToTest);
            Cleanup();

        }
        [Test]
        [Order(2)]
        public void Login()
        {
            Setup();
            LoginTest logintest = new LoginTest();
            logintest.MPGSiteLoginTest(driver, builder, testResults, MailIdToTest);
        }
        //[Test]
        //[Order(6)]
        //public void PasswordReset()
        //{
        //    //Forgot Password flow
        //    Setup();
        //    PasswordResetTest passwordReset = new PasswordResetTest();
        //    passwordReset.MPGSitePasswordReset(driver, builder, testResults, MailIdToTest);
        //    Cleanup();
        //}
        //[Test]
        //[Order(3)]
        //public void PasswordUpdate()
        //{
        //    AccountDetailsUpdate accountDetailsUpdate = new AccountDetailsUpdate();
        //    ProfileEditFlag = "Password";
        //    accountDetailsUpdate.HoverUserIcon(driver, builder, ProfileEditFlag, testResults);
        //    // Cleanup();
        //}
        ////[Test]
        ////[Order(4)]
        ////public void PhoneNumberUpdate()
        ////{
        ////    AccountDetailsUpdate accountDetailsUpdate = new AccountDetailsUpdate();
        ////    ProfileEditFlag = "Phone";
        ////    accountDetailsUpdate.HoverUserIcon(driver, builder, ProfileEditFlag, testResults);
        ////}
        //[Test]
        //[Order(5)]
        //public void ProfileDetailsUpdate()
        //{
        //    AccountDetailsUpdate accountDetailsUpdate = new AccountDetailsUpdate();
        //    ProfileEditFlag = "Profile";
        //    accountDetailsUpdate.HoverUserIcon(driver, builder, ProfileEditFlag, testResults);
        //    Cleanup();
        //}

        [OneTimeTearDown]
        public void SaveTestResults()
        {
            string url = ReadFromExcel.SiteURL;
            ReadFromExcel.SiteDetailsDic.Remove(url);
            ReadFromExcel.SignupDetailsDic.Remove(url);
           
            string resultFilePath = "C:\\Users\\Merlin.Savarimuthu\\Reports\\EmailSetting\\SiteCheck_TestResult.xlsx";

            WriteExcelTestResult writeTestResult = new WriteExcelTestResult();
            writeTestResult.WriteTestResultsToExcel(resultFilePath, testResults);
          
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
           
            driver.Quit();
            Thread.Sleep(5000);
        }
        //public static void SendEmail(string smtpServer)
        //{
        //    //Send teh High priority Email  
        //    SendEmail mailMan = new SendEmail(smtpServer);

        //    EmailSendConfigure myConfig = new EmailSendConfigure();
        //    // replace with your email userName  
        //    myConfig.ClientCredentialUserName = "merlinsavarimuthu@gmail.com";
        //    // replace with your email account password
        //    myConfig.ClientCredentialPassword = "Merl@1505";
        //    myConfig.TOs = new string[] { "merlinsavarimuthu@gmail.com" };
        //    myConfig.CCs = new string[] { };
        //    myConfig.From = "merlinsavarimuthu@gmail.com";
        //    myConfig.FromDisplayName = "Merlin";
        //    myConfig.Priority = System.Net.Mail.MailPriority.Normal;
        //    myConfig.Subject = "Daily site health check test";

        //    EmailContent myContent = new EmailContent();
        //    myContent.Content = "This is a test message from DailySite Check Application";

        //    mailMan.SendMail(myConfig, myContent);
        //}
    public static void SendTestmail()
        {

            var fromAddress = new MailAddress("merlinsavarimuthu@gmail.com", "Merlin");
            var toAddress = new MailAddress("merlinbenny1505@gmail.com", "Merlin Benny");
            string fromPassword = "Merl@1505";
            string subject = "Test email";
            string body = "This is a test email from Selenium c# Daily site check automation application";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 60000,
            };
            try
            {
                //await smtp.SendMailAsync("merlin.savarimuthu@gmail.com", "merlinbenny1505@gmail.com",
                //                        "test subject", "test body mail");
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Test email send", ex.Message);
            }
            
        }
        //public static void SendEmailViaWeb()
        //{
        //    IWebDriver Emaildriver = new ChromeDriver();
        //    Emaildriver.Navigate().GoToUrl("https://outlook.office.com/");
        //    // Navigate to the email compose page
        //    Emaildriver.FindElement(By.ClassName("root-191")).Click();

        //    // Fill in the recipient, subject, and email body
        //    Emaildriver.FindElement(By.ClassName("aoWYQ")).SendKeys("merlin.savarimuthu@manpowergroup.com");
        //    Emaildriver.FindElement(By.ClassName("g7toD")).SendKeys("merlin.savarimuthu@manpowergroup.com");
        //    Emaildriver.FindElement(By.ClassName("ms-TextField-field")).SendKeys("Test Subject"); 
        //    Emaildriver.FindElement(By.ClassName("DziEn")).SendKeys("This is the email body.");

        //    // Click the "Send" button
        //    Emaildriver.FindElement(By.ClassName("be51T")).Click();

        //    Emaildriver.Quit();
        //}

    }

  public class Program
    {
        public static Dictionary<string, Dictionary<string, string>> SignupDetails = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> SiteDetails = new Dictionary<string, Dictionary<string, string>>();

        static void Main(string[] args)
        {
            SendEmail sendmail = new SendEmail("test");
            sendmail.OpenOutlookAndDraftEmail();
            ReadFromExcel.ReturnSiteData();
            SiteDetails = ReadFromExcel.SiteDetailsDic;
            int siteDetailsCount = SiteDetails.Count;
            SignupDetails = ReadFromExcel.SignupDetailsDic;

            for (int i = 0; i < siteDetailsCount; i++)
            {
                var testAssembly = Assembly.GetExecutingAssembly();
                try
                {
                    var autoRun = new AutoRun(testAssembly);
                    autoRun.Execute(args);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Thread.Sleep(2000);
            }

        }

    }
}
