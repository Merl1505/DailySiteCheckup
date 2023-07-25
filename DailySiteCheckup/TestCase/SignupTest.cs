using DailySiteCheckup.Feature;
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
        public void MPGSiteSignupTest(IWebDriver driver, Actions builder)
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
            driver.FindElement(By.Id("email")).SendKeys("sitehealthtest14@mailsac.com");

            //click send verification code 
            IAction SendEmailVerification_action = builder.Click(driver.FindElement(By.Id("emailVerificationControl_but_send_code"))).Build();
            SendEmailVerification_action.Perform();
            Thread.Sleep(3500);

            // enter otp (temporarily ask for otp to enter manually)
            TestContext.Progress.WriteLine("Enter the Email Verification Code for SignupTest.....");
            string Signup_EmailVerificationCode = Console.ReadLine();
            driver.FindElement(By.Id("VerificationCode")).SendKeys(Signup_EmailVerificationCode);
            //get the otp through mailsac API end points



            //click on verify code button
            // Wait for the element to become interactable
            //IWebElement element = null;
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //bool interactable = wait.Until(driver =>
            //{
            //    try
            //    {
            //        element = driver.FindElement(By.Id("emailVerificationControl_but_verify_code"));
            //        return element.Enabled && element.Displayed;
            //    }
            //    catch (NoSuchElementException)
            //    {
            //        return false;
            //    }
            //    catch (StaleElementReferenceException)
            //    {
            //        return false;
            //    }
            //});
            //if (interactable)
            //{
            //    // Perform your actions on the element
            //    element.Click();
            //}
            IAction verifycode_action = builder.Click(driver.FindElement(By.Id("emailVerificationControl_but_verify_code"))).Build();
            verifycode_action.Perform();
            Thread.Sleep(3500);

            // enter other fileds
            /* these fields has to be populated from an excel containing
             Mail id, password, fname and surname  */
            driver.FindElement(By.Id("newPassword")).SendKeys("Test@123");
            driver.FindElement(By.Id("givenName")).SendKeys("testfname");
            driver.FindElement(By.Id("surname")).SendKeys("testsname");

            // click on check box and radio buttons
            IAction actionchkbox = builder.Click(driver.FindElement(By.Id("extension_TermsOfUseConsented_True"))).Build();
            actionchkbox.Perform();

            //click on submit
            IAction submitAction = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            submitAction.Perform();
            Thread.Sleep(5000);

            // get test result
            IWebElement labelAccountCreated = (IWebElement)driver.FindElement(By.XPath("//div[@class = 'attrEntry']/label[@for = 'successhdg']"));
            bool account_created_txt = labelAccountCreated.Text.Contains("Your account has been created");
            Assert.IsTrue(account_created_txt);
            if (account_created_txt)
            {
                TestContext.Progress.WriteLine("Signup Success.....");
                tests.testResults.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "Y", Message = "Success", TestCaseName = "SignUp" });
            }
            else
            {
                TestContext.Progress.WriteLine("Signup Failed.....");
                tests.testResults.Add(new TestResult { SiteName = "https://www.experis.com/", Status = "Y", Message = "Success", TestCaseName = "SignUp" });
            }


        }


    }
}
