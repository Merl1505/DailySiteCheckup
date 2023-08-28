﻿using DailySiteCheckup.Feature;
using DailySiteCheckup.TestCase;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnitLite;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Reflection;
using System;
using System.IO;
using System.Runtime;

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
        public SeleniumTests()
        {
            testResults = new List<TestResult>();
           
            //SiteDetailsDictionary = new Dictionary<string, Dictionary<string, string>>();
        }
        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito"); // opens Chrome in incognito mode

            //Set up the ChromeDriver
            driver = new ChromeDriver(options);
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
        [Test]
        [Order(6)]
        public void PasswordReset()
        {
            //Forgot Password flow
            Setup();
            PasswordResetTest passwordReset = new PasswordResetTest();
            passwordReset.MPGSitePasswordReset(driver, builder, testResults, MailIdToTest);
            Cleanup();
        }
        [Test]
        [Order(3)]
        public void PasswordUpdate()
        {
            AccountDetailsUpdate accountDetailsUpdate = new AccountDetailsUpdate();
            ProfileEditFlag = "Password";
            accountDetailsUpdate.HoverUserIcon(driver, builder, ProfileEditFlag, testResults);
            // Cleanup();
        }
        [Test]
        [Order(4)]
        public void PhoneNumberUpdate()
        {
            AccountDetailsUpdate accountDetailsUpdate = new AccountDetailsUpdate();
            ProfileEditFlag = "Phone";
            accountDetailsUpdate.HoverUserIcon(driver, builder, ProfileEditFlag, testResults);
        }
        [Test]
        [Order(5)]
        public void ProfileDetailsUpdate()
        {
            AccountDetailsUpdate accountDetailsUpdate = new AccountDetailsUpdate();
            ProfileEditFlag = "Profile";
            accountDetailsUpdate.HoverUserIcon(driver, builder, ProfileEditFlag, testResults);
        }

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
        }
       
    }

  public class Program
    {
        public static Dictionary<string, Dictionary<string, string>> SignupDetails = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> SiteDetails = new Dictionary<string, Dictionary<string, string>>();

        static void Main(string[] args)
        {
            //SendEmail.SendReportViaOutlookMail();
            ReadFromExcel.ReturnSiteData();
            SiteDetails = ReadFromExcel.SiteDetailsDic;

            SignupDetails = ReadFromExcel.SignupDetailsDic;

            for (int i = 0; i < SiteDetails.Count; i++)
            {
                var testAssembly = Assembly.GetExecutingAssembly();
                var autoRun = new AutoRun(testAssembly);
                autoRun.Execute(args);
                Thread.Sleep(2000);
            }

        }
    }
}
