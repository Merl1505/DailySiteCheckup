using DailySiteCheckup.Feature;
using DailySiteCheckup.TestCase;
using NUnit.Framework;
using NUnitLite;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Reflection;

namespace SeleniumNUnitConsoleApp
{
    [TestFixture]
    public class SeleniumTests
    {
        private IWebDriver driver;
        private Actions builder;
        public List<TestResult> testResults { get; set; }
        public static string ?ProfileEditFlag { get; set; }

        public SeleniumTests()
        {
            testResults = new List<TestResult>();
        }
        [OneTimeSetUp]
        public void Setup()
        {
            //Set up the ChromeDriver
            driver = new ChromeDriver();
            builder = new Actions(driver);
        }

        [Test]
        [Order(1)]
        public void SignupTest()
        {
            SignupTest signupTest = new SignupTest();
            signupTest.MPGSiteSignupTest(driver, builder);
            Cleanup();

        }
        [Test]
        [Order(2)]
        public void Login()
        {
            Setup();
            LoginTest logintest = new LoginTest();
            logintest.MPGSiteLoginTest(driver, builder, testResults);
        }
        [Test]
        [Order(6)]
        public void PasswordReset()
        {
            //Forgot Password flow
            Setup();
            PasswordResetTest passwordReset = new PasswordResetTest();
            passwordReset.MPGSitePasswordReset(driver, builder);
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
            DateTime currentDate = DateTime.Now;
            string currdate = currentDate.ToShortDateString();
            string date_For_File_Path = currdate.Replace("/", "");

            string resultFilePath = "C:\\Users\\Merlin.Savarimuthu\\Reports\\TestReport_" + date_For_File_Path;

            WriteExcelTestResult writeTestResult = new WriteExcelTestResult();
            writeTestResult.WriteTestResultsToExcel(resultFilePath, testResults);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            //ClearCacheClass clearCache = new ClearCacheClass();
            //clearCache.Clearbrowsercache();

            // Clean up the driver
            Console.ReadLine();
            driver.Quit();
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            //ReadFromExcel readFromExcel = new ReadFromExcel();
            //readFromExcel.GroupSiteandColumns("path");
            ReadEmailForOtp readEmail = new ReadEmailForOtp();
            Task callAsyncReadEmailTask = readEmail.ReadMailsacEmailAPIAsync();
            callAsyncReadEmailTask.Wait(); // wait for the async method to get completed

            var testAssembly = Assembly.GetExecutingAssembly();
            //Console.ReadLine();
            return new AutoRun(testAssembly).Execute(args);
        }
    }
}
