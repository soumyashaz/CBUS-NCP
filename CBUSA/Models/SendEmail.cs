using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Web.UI;
namespace CBUSA.Models
{

    public interface IEmailSend
    {
        bool Send(string Subject, string Body, string MailTo,string EmailFrom);

    }

    public class SendEmail : IEmailSend
    {

        public bool Send(string Subject, string Body, string MailTo,string MailFrom)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
                mail.To.Add(MailTo);

                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = false;
                // your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["MailServer"];
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["Email"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSLEnabled"].ToString());
                smtp.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


    public class SendEmailHtml : IEmailSend
    {
        public bool Send(string Subject, string Body, string MailTo,string MailFrom)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
                mail.From = new MailAddress(MailFrom);
                mail.To.Add(MailTo);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                // your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["MailServer"];
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["Email"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
               
              
               
               
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSLEnabled"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);
                return true;
            }
            catch (Exception)
            {
               
                
               return false;
            }
               

        }
        
    }
}