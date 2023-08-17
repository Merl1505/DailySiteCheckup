using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailySiteCheckup.Feature;
using System.Xml.Linq;
using SeleniumNUnitConsoleApp;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace DailySiteCheckup.TestCase
{
    public class AccountDetailsUpdate
    {
        // SeleniumTests tests = new SeleniumTests();

        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        public void AccountSettings(IWebDriver driver, Actions builder, string ProfileEditFlag)
        {

            string url = "https://www.experis.com/";
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
            Thread.Sleep(6000);
            // check if cookies popup appear
            //IWebElement CookiePopupelement = driver.FindElement(By.Id("onetrust-group-container"));
            //string CookiePopupAttr = CookiePopupelement.GetAttribute("id");
            //if (CookiePopupAttr == "onetrust-group-container")
            //{
            //    IAction cookie_accept_action = builder.Click(driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Build();
            //    cookie_accept_action.Perform();
            //    Thread.Sleep(1000);

            //}
            //Click action - signup link click
            IAction user_iconclick_action = builder.Click(driver.FindElement(By.ClassName("user-icon"))).Build();
            user_iconclick_action.Perform();
            Thread.Sleep(8000);

            //login 
            //LoginTest login = new LoginTest();
            //login.EnterCredentials(driver, builder);
            //Thread.Sleep(3000);

        }
        public void HoverUserIcon(IWebDriver driver, Actions builder, string ProfileEditFlag, List<TestResult> tests)
        {
            //hover on user icon button
            IWebElement elementToHover = driver.FindElement(By.ClassName("user-icon"));
            builder.MoveToElement(elementToHover).Perform();
            // click on third menu 
            IWebElement elements = driver.FindElement(By.Id("dp-menu"));
            IReadOnlyCollection<IWebElement> newelementList = elements.FindElements(By.TagName("li"));
            List<IWebElement> elementList = new List<IWebElement>(newelementList);
            if (elementList.Count >= 3)
            {
                elementList[2].Click();
                Thread.Sleep(5000);
            }
            // expand account section
            IAction password_change_action = builder.Click(driver.FindElement(By.Id("form0"))).Build();
            password_change_action.Perform();
            Thread.Sleep(1000);
            if (ProfileEditFlag == "Password")
                ChangePassword(driver, builder, tests);
            else if (ProfileEditFlag == "Phone")
                ChangePhone(driver, builder, tests);
            else if (ProfileEditFlag == "Profile")
                ChangeProfileDetails(driver, builder, tests);
        }
        public void ChangePassword(IWebDriver driver, Actions builder, List<TestResult> tests)
        {
            TestContext.Progress.WriteLine("Execute Password Update Test.....");
            //Find the element inside third div and click the button inside that div
            IWebElement button = driver.FindElement(By.XPath("(//div[@class = 'account-edit-block'])[3]//button[@class = 'primary-button']"));
            IAction update_pwd_action = builder.Click(button).Build();
            update_pwd_action.Perform();
            Thread.Sleep(6000);

            //update the password to input fields
            driver.FindElement(By.Id("oldPassword")).SendKeys(configuration["FirstPassword"]);
            driver.FindElement(By.Id("newPassword")).SendKeys(configuration["PasswordUpdate"]);
            driver.FindElement(By.Id("reenterPassword")).SendKeys(configuration["PasswordUpdate"]);

            //click on continue
            IAction continue_action = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            continue_action.Perform();
            Thread.Sleep(20000);

            //check if user-icon is present return assert true
            IWebElement element = driver.FindElement(By.ClassName("login"));
            // Get the value of the "class" attribute
            string classAttributeValue = element.GetAttribute("class");
            // Check if the class attribute value contains the desired class
            bool hasDesiredClass = classAttributeValue.Contains("logged-in");
            Assert.IsTrue(classAttributeValue.Contains("logged-in"));
            if (hasDesiredClass)
            {
                TestContext.Progress.WriteLine("Password updated successfully.....");
                tests[0].UpdatePwdStatus = "Y";
                //tests.Add(new TestResult { SiteName = "https://www.experis.com/", UpdatePwdStatus = "Y", Message = "Success", TestCaseName = "Password Update" });
            }
            else
            {
                TestContext.Progress.WriteLine("Password update Failed.....");
                tests[0].UpdatePwdStatus = "N";
                //tests.Add(new TestResult { SiteName = "https://www.experis.com/", UpdatePwdStatus = "N", Message = "Fail", TestCaseName = "Password Update" });
            }
            // ChangePhone(driver, builder);
            TestContext.Progress.WriteLine("Password updated successfully.....");

        }
        public void ChangePhone(IWebDriver driver, Actions builder, List<TestResult> tests)
        {
            TestContext.Progress.WriteLine("Execute Phone Number update Test.....");
            //Find the element inside third div and click the button inside that div
            IWebElement button = driver.FindElement(By.XPath("(//div[@class = 'account-edit-block'])[2]//button[@class = 'primary-button']"));
            IAction update_pwd_action = builder.Click(button).Build();
            update_pwd_action.Perform();
            Thread.Sleep(5000);

            //click on country dropdow
            IWebElement cntry_dropdown = driver.FindElement(By.Id("countryCode"));
            cntry_dropdown.Click();
            IWebElement option = driver.FindElement(By.XPath("//option[@value='+91']"));
            option.Click();
            driver.FindElement(By.Id("number")).SendKeys("9842893449");

            // function to send verification code 
            IAction sendcode_action = builder.Click(driver.FindElement(By.Id("sendCode"))).Build();
            sendcode_action.Perform();
            TestContext.Progress.WriteLine("Enter the OTP sent to the mobile number.....");
            string phoneNumChange_ver_code = Console.ReadLine();
            driver.FindElement(By.Id("verificationCode")).SendKeys(phoneNumChange_ver_code);
            IAction confirmCode_action = builder.Click(driver.FindElement(By.Id("verifyCode"))).Build();
            confirmCode_action.Perform();
            Thread.Sleep(20000);
            // function to call me button

            //check if user-icon is present return assert true
            IWebElement element = driver.FindElement(By.ClassName("login"));
            // Get the value of the "class" attribute
            string classAttributeValue = element.GetAttribute("class");
            // Check if the class attribute value contains the desired class
            bool hasDesiredClass = classAttributeValue.Contains("logged-in");
            Assert.IsTrue(classAttributeValue.Contains("logged-in"));
            if (hasDesiredClass)
            {
                TestContext.Progress.WriteLine("Phone Number Updated Successfully.....");
                tests[0].UpdatePhoneStatus = "Y";
                // tests.Add(new TestResult { SiteName = "https://www.experis.com/", UpdatePhoneStatus = "Y", Message = "Success", TestCaseName = "Phone Update" });
            }
            else
            {
                TestContext.Progress.WriteLine("Phone Number Update Failed.....");
                tests[0].UpdatePhoneStatus = "N";
                //tests.Add(new TestResult { SiteName = "https://www.experis.com/", UpdatePhoneStatus = "N", Message = "Fail", TestCaseName = "Phone Update" });
            }


        }
        public void ChangeProfileDetails(IWebDriver driver, Actions builder, List<TestResult> tests)
        {
            //Update first name and last names
            TestContext.Progress.WriteLine("Execute Names update Test.....");
            //Find the element inside third div and click the button inside that div
            IWebElement button = driver.FindElement(By.XPath("(//div[@class = 'account-edit-block'])[1]//button[@class = 'primary-button']"));
            IAction update_pwd_action = builder.Click(button).Build();
            update_pwd_action.Perform();
            Thread.Sleep(5000);

            IWebElement fname = driver.FindElement(By.Id("givenName"));
            IWebElement sname = driver.FindElement(By.Id("surname"));
            fname.Clear(); sname.Clear();
            fname.SendKeys(configuration["ResetFirstName"]);
            sname.SendKeys(configuration["ResetSecondName"]);

            //click on continue
            IAction submitAction = builder.Click(driver.FindElement(By.Id("continue"))).Build();
            submitAction.Perform();
            Thread.Sleep(20000);

            //check if user-icon is present return assert true
            IWebElement element = driver.FindElement(By.ClassName("login"));
            // Get the value of the "class" attribute
            string classAttributeValue = element.GetAttribute("class");
            // Check if the class attribute value contains the desired class
            bool hasDesiredClass = classAttributeValue.Contains("logged-in");
            Assert.IsTrue(hasDesiredClass);
            if (hasDesiredClass)
            {
                TestContext.Progress.WriteLine("First Name and Last Name updated Successfully.....");
                tests[0].ProfileEditStatus = "Y";
                //tests.Add(new TestResult { SiteName = "https://www.experis.com/", ProfileEditStatus = "Y", Message = "Success", TestCaseName = "Profile Update" });
            }
            else
            {
                TestContext.Progress.WriteLine("Names update failed.....");
                tests[0].ProfileEditStatus = "N";
                //tests.Add(new TestResult { SiteName = "https://www.experis.com/", ProfileEditStatus = "N", Message = "Fail", TestCaseName = "Profile Update" });
            }

        }
    }
}
