﻿using DailySiteCheckup.Helper;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace DailySiteCheckup.Feature
{
    public class SendEmail
    {
        private string m_HostName;
        public SendEmail(string hostname)
        {
            m_HostName = hostname;
        }
        //public void SendMail(EmailSendConfigure emailConfig, EmailContent content)
        //{
        //    MailMessage msg = ConstructEmailMessage(emailConfig, content);
        //    Send(msg, emailConfig);
        //}
        //private MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        //{
        //    MailMessage msg = new System.Net.Mail.MailMessage();
        //    foreach (string to in emailConfig.TOs)
        //    {
        //        if (!string.IsNullOrEmpty(to))
        //        {
        //            msg.To.Add(to);
        //        }
        //    }

        //    foreach (string cc in emailConfig.CCs)
        //    {
        //        if (!string.IsNullOrEmpty(cc))
        //        {
        //            msg.CC.Add(cc);
        //        }
        //    }

        //    msg.From = new MailAddress(emailConfig.From,
        //                               emailConfig.FromDisplayName,
        //                               System.Text.Encoding.UTF8);
        //    msg.IsBodyHtml = content.IsHtml;
        //    msg.Body = content.Content;
        //    msg.Priority = emailConfig.Priority;
        //    msg.Subject = emailConfig.Subject;
        //    msg.BodyEncoding = System.Text.Encoding.UTF8;
        //    msg.SubjectEncoding = System.Text.Encoding.UTF8;

        //    if (content.AttachFileName != null)
        //    {
        //        Attachment data = new Attachment(content.AttachFileName,
        //                                         MediaTypeNames.Application.Zip);
        //        msg.Attachments.Add(data);
        //    }

        //    return msg;
        //}
        //private void Send(MailMessage message, EmailSendConfigure emailConfig)
        //{
        //    SmtpClient client = new SmtpClient();
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = new System.Net.NetworkCredential(
        //                          emailConfig.ClientCredentialUserName,
        //                          emailConfig.ClientCredentialPassword);
        //    client.Host = m_HostName;
        //    client.Port = 587; 
        //    client.EnableSsl = true;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;

        //    try
        //    {
        //        client.Send(message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error in Send email: {0}", e.Message);
        //        throw;
        //    }
        //    message.Dispose();
        //}

        public void OpenOutlookAndDraftEmail()
        {
            // Initialize Chrome WebDriver
            IWebDriver driver = new ChromeDriver();

            // Open Outlook
            Application outlookApp = new Application();
            MailItem mailItem = outlookApp.CreateItem(OlItemType.olMailItem) as MailItem;

            // Draft the email
            mailItem.Subject = "Test Subject";
            mailItem.To = "merlin.savarimuthu@manpowergorup.com";
            mailItem.Body = "this is sample body";

            // Display Outlook
            mailItem.Display();
            // Switch to the Outlook window (assuming only one instance of Outlook is running)
            driver.SwitchTo().Window(driver.WindowHandles[1]); // Assuming Outlook window is the second window

            // Close the Chrome WebDriver
            driver.Quit();

        }
    }
}
