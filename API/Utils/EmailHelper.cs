using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Utils
{
    public class EmailHelper
    {
        public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("email@google.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("email@google.com", "yourpassword");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
    }
}
