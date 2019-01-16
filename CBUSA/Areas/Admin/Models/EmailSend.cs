using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace CBUSA.Areas.Admin.Models
{
    public class EmailSend : IEmailSend
    {
        public bool Send(string Subject, string Body, string MailTo, string Bcc = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
              
                 mail.To.Add(MailTo);
                //StreamReader reader = new StreamReader(Path);
                //string readFile = reader.ReadToEnd();
                //string StrContent = "";
                //StrContent = readFile;
                ////Here replace the name with [MyName]
                //StrContent = StrContent.Replace("{MyName}",);
                //StrContent = StrContent.Replace("{message}",);
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

        public bool SendWithAttachedment(string Subject, string Body, string MailTo, String AttachedMent)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
                mail.To.Add(MailTo);
                //StreamReader reader = new StreamReader(Path);
                //string readFile = reader.ReadToEnd();
                //string StrContent = "";
                //StrContent = readFile;
                //Here replace the name with [MyName]

                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
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
}

public interface IEmailSend
{
    bool Send(string Subject, string Body, string MailTo, string Bcc = "");
    bool SendWithAttachedment(string Subject, string Body, string MailTo, String AttachedMent);
}


public class EmailSendHtml : IEmailSend
{
    public bool Send(string Subject, string Body, string MailTo,string Bcc="")
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);

            string[] strArrayTo = MailTo.Split(',');
           
            for (int i = 0; i < strArrayTo.Count(); i++)
            {
                mail.To.Add(strArrayTo[i]);
            }
            if(!string.IsNullOrEmpty(Bcc))
            {
                string[] strArrayBcc = Bcc.Split(',');
                for (int i = 0; i < strArrayBcc.Count(); i++)
                {
                    mail.Bcc.Add(strArrayBcc[i]);
                }
            }
            

           // mail.To.Add(MailTo);
            //StreamReader reader = new StreamReader(Path);
            //string readFile = reader.ReadToEnd();
            //string StrContent = "";
            //StrContent = readFile;
            //Here replace the name with [MyName]

            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
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
        catch (Exception )
        {
            return false;
        }
    }

    public bool SendWithAttachedment(string Subject, string Body, string MailTo,String AttachedMent)
    {
        try
        {
            StringReader sr = new StringReader(AttachedMent.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
                mail.To.Add(MailTo);
                //StreamReader reader = new StreamReader(Path);
                //string readFile = reader.ReadToEnd();
                //string StrContent = "";
                //StrContent = readFile;
                //Here replace the name with [MyName]

                mail.Subject = Subject;
                mail.Body = Body;
                mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "Referral.pdf"));
                mail.IsBodyHtml = true;
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
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}


