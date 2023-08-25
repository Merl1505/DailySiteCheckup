using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;

namespace DailySiteCheckup.Feature
{
    public class SendEmail
    {
        public static void SendReportViaOutlookMail()
        {
            // Sender's email credentials
            string senderEmail = "merlintest155@mailsac.com";
            string senderPassword = "Jebin@1003MPG";

            // Recipient's email address
            string recipientEmail = "merlin.savarimuthu@manpowergroup.com";

            // Create a new MailMessage
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = "Test Email";
            mail.Body = "This is a test email sent from Daily site Check Application.";

            // Set up the SMTP client
            SmtpClient smtpClient = new SmtpClient("mailsac-smtp-server.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }

    }
}
