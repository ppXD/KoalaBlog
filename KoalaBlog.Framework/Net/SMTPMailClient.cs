using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Net
{
    public class SMTPMailClient
    {
        /// <summary>
        /// Sends an e-mail message using the designated SMTP mail server.
        /// </summary>
        /// <param name="subject">The subject of the message being sent.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="fromAddress">The sender's e-mail address.</param>
        /// <param name="toAddress">The recipient's e-mail address (separate multiple e-mail addresses
        /// with a semi-colon).</param>
        /// <param name="ccAddress">The address(es) to be CC'd (separate multiple e-mail addresses with
        /// a semi-colon).</param>
        /// <remarks>You must set the SMTP server within this method prior to calling.</remarks>
        /// <example>
        /// <code>
        ///   // Send a quick e-mail message
        ///   SendEmail.SendMessage("This is a Test", 
        ///                         "This is a test message...",
        ///                         "noboday@nowhere.com",
        ///                         "somebody@somewhere.com", 
        ///                         "ccme@somewhere.com");
        /// </code>
        /// </example>
        public static void SendMessage(string subject, string messageBody, string toAddressSemiCommaSept, string ccAddressSemiCommaSept, bool isHtml, bool sendAsync)
        {
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();

            // Set the sender's address
            //message.From = new MailAddress(fromAddress);

            // Allow multiple "To" addresses to be separated by a semi-colon
            if (toAddressSemiCommaSept.Trim().Length > 0)
            {
                foreach (string addr in toAddressSemiCommaSept.Split(';'))
                {
                    message.To.Add(new MailAddress(addr));
                }
            }

            // Allow multiple "Cc" addresses to be separated by a semi-colon
            if (ccAddressSemiCommaSept.Trim().Length > 0)
            {
                foreach (string addr in ccAddressSemiCommaSept.Split(';'))
                {
                    message.CC.Add(new MailAddress(addr));
                }
            }

            // Set the subject and message body text
            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = isHtml;
            // TODO: *** Modify for your SMTP server ***
            // Set the SMTP server to be used to send the message
            //client.EnableSsl = true;
            if (sendAsync)
            {
                object userState = message;

                //wire up the event for when the Async send is completed
                client.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);

                // Send the e-mail message
                client.SendAsync(message, userState);
            }
            else
            {
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;

            //write out the subject
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                Console.WriteLine("Send canceled for mail with subject [{0}].", subject);
            }
            if (e.Error != null)
            {
                Console.WriteLine("Error {1} occurred when sending mail [{0}] ", subject, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message [{0}] sent.", subject);
            }
        }

        public static Task SendAsync(string subject, string messageBody, string toAddressSemiCommaSept, string ccAddressSemiCommaSept, bool isHtml)
        {
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();

            // Set the sender's address
            //message.From = new MailAddress(fromAddress);

            // Allow multiple "To" addresses to be separated by a semi-colon
            if (toAddressSemiCommaSept.Trim().Length > 0)
            {
                foreach (string addr in toAddressSemiCommaSept.Split(';'))
                {
                    message.To.Add(new MailAddress(addr));
                }
            }

            // Allow multiple "Cc" addresses to be separated by a semi-colon
            if (ccAddressSemiCommaSept.Trim().Length > 0)
            {
                foreach (string addr in ccAddressSemiCommaSept.Split(';'))
                {
                    message.CC.Add(new MailAddress(addr));
                }
            }

            // Set the subject and message body text
            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = isHtml;
            // TODO: *** Modify for your SMTP server ***
            // Set the SMTP server to be used to send the message
            //client.EnableSsl = true;

            // Send the e-mail message async
            return client.SendMailAsync(message);
        }

    }
}
