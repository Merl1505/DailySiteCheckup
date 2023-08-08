using DailySiteCheckup.Feature;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumNUnitConsoleApp;

namespace DailySiteCheckup.TestCase
{

    public class SignupTest
    {
        SeleniumTests tests = new SeleniumTests();
        //ReadFromExcel readFromExcel = new ReadFromExcel();
        //string test = readFromExcel.dataDictionary["1-1"];

        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        public void MPGSiteSignupTest(IWebDriver driver, Actions builder,string emailId)
        {
            TestContext.Progress.WriteLine("Execute Signup Test.....");
            NavigateToSite navigateToSite = new NavigateToSite();
            navigateToSite.NavigateToHomePage(driver, builder);

            // Assert.IsTrue(contactUsPageHeader.Text.Contains("Create an Account"));
            //click on signup link
            IAction signuplinkClick = builder.Click(driver.FindElement(By.Id("createAccount"))).Build();
            signuplinkClick.Perform();
            Thread.Sleep(6000);
            /* 1. send site name to ReadFromExcel and get the column names(input id for signupform)
                  and its values grouped as per the site. Get only the site that is tested at the moment. 
               2. get the column name from each group and pass each column name in a for loop
               3. check Y/N for that column, If yes allow inside
                    3.a. get the input id of that particular column
                    3.b. based on the input id send keys to that particular inputs and fill the signup form.
               4. If no, send back to for loop
            */

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

            IAction verifycode_action = builder.Click(driver.FindElement(By.Id("emailVerificationControl_but_verify_code"))).Build();
            verifycode_action.Perform();
            Thread.Sleep(3500);

            // enter other fileds
            /* these fields has to be populated from an excel containing
             Mail id, password, fname and surname  */
            driver.FindElement(By.Id("newPassword")).SendKeys(configuration["FirstPassword"]);
            driver.FindElement(By.Id("givenName")).SendKeys(configuration["FirstName"]);
            driver.FindElement(By.Id("surname")).SendKeys(configuration["SecondName"]);

            //// click on check box and radio buttons
            //IAction actionchkbox = builder.Click(driver.FindElement(By.Id("extension_TermsOfUseConsented_True"))).Build();
            //actionchkbox.Perform();

            //click on submit
            IAction submitAction = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            submitAction.Perform();
            Thread.Sleep(5000);
            //find if error has occured 
            IWebElement labelerror = (IWebElement)driver.FindElement(By.Id("claimVerificationServerError"));
            bool IsError = labelerror.Text.Contains("incorrect.");
            bool account_created_txt;
            // get test result
            if (!IsError)
            {
                IWebElement labelAccountCreated = (IWebElement)driver.FindElement(By.XPath("//div[@class = 'attrEntry']/label[@for = 'successhdg']"));
                account_created_txt = labelAccountCreated.Text.Contains("Your account has been created");
                Assert.IsTrue(account_created_txt);
                if (account_created_txt)
                {
                    TestContext.Progress.WriteLine("Signup Success.....");
                    tests.testResults.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "Y", Message = "Success", TestCaseName = "SignUp" });
                }
               
            }

            else
            {
                TestContext.Progress.WriteLine("Signup Failed.....Incorrect username and password combinations");
                tests.testResults.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "Y", Message = "Success", TestCaseName = "SignUp" });
            }


        }


    }
}
