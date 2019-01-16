using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
//using MedullusSendGridEmailLib;

namespace CBUSA.Models
{
    public interface IEmailSendApi
    {
        bool Send(string Subject, string Body, string MailTo, string EmailSenderId);
        bool Send(string Subject, string Body, string[] MailTo, string EmailSenderId);
        bool SendAll(string Subject, string Body, string[] MailTo);
    }

    public class EmailSendApi : IEmailSendApi
    {
        public bool Send(string Subject, string Body, string MailTo, string EmailSenderId)
        {
            //throw new NotImplementedException();
            try
            {
                var EmailDetails = new List<dynamic> { new { EmailReceiverId = MailTo } };
                var param = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    emailHeader = new
                    {
                        EmailSenderId = EmailSenderId,
                        EmailSubject = Subject,
                        EmailBody = Body,
                        ClientId = 1,//NR
                        EmailSenderPasswd = "1",//NR
                        APIKey = ConfigurationManager.AppSettings["ApiKey"]
                    },
                    emailDetails = EmailDetails
                });
                //HttpContent ContentPost = new StringContent(param, Encoding.UTF8, "application/json");
                //using (HttpClient Client = new HttpClient())
                //{
                //    // client.BaseAddress = new Uri(apiUrl);
                //    Client.DefaultRequestHeaders.Accept.Clear();
                //    Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //    var Response = Client.PostAsync(string.Format("{0}/{1}", ConfigurationManager.AppSettings["SendEmailApiUrl"],
                //        ConfigurationManager.AppSettings["SendEmailMethod"]
                //        ), ContentPost).Result;
                //    if (Response.IsSuccessStatusCode)
                //    {
                //        var data = Response.Content.ReadAsStringAsync().ToString();
                //        //if (data == "Success")
                //        //{
                //        return true;
                //        // }
                //    }

                JObject Parameters = JObject.Parse(@"" + param);
                MedullusSendGridEmailLib.MedullusSendGridEmailLib sendMail = new MedullusSendGridEmailLib.MedullusSendGridEmailLib();
                var result = sendMail.SendStaticEmailBySendGrid(Parameters);
                return result.ToLower() == "success" ? true : false;
                // return false;
                // }
                // return false;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool Send(string Subject, string Body, string[] MailTo, string EmailSenderId)
        {
            //throw new NotImplementedException();
            try
            {
                var EmailDetails = MailTo.Select(x => new { EmailReceiverId = x });
                var param = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    emailHeader = new
                    {
                        EmailSenderId = EmailSenderId,
                        EmailSubject = Subject,
                        EmailBody = Body,
                        ClientId = 1,//NR
                        EmailSenderPasswd = "1",//NR
                        APIKey = ConfigurationManager.AppSettings["ApiKey"]
                    },
                    emailDetails = EmailDetails
                });
                //HttpContent ContentPost = new StringContent(param, Encoding.UTF8, "application/json");
                //using (HttpClient Client = new HttpClient())
                //{
                //    // client.BaseAddress = new Uri(apiUrl);
                //    Client.DefaultRequestHeaders.Accept.Clear();
                //    Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //    var Response = Client.PostAsync(string.Format("{0}/{1}", ConfigurationManager.AppSettings["SendEmailApiUrl"],
                //        ConfigurationManager.AppSettings["SendEmailMethod"]
                //        ), ContentPost).Result;
                //    if (Response.IsSuccessStatusCode)
                //    {
                //        var data = Response.Content.ReadAsStringAsync().ToString();
                //        //if (data == "Success")
                //        //{
                //        return true;
                //        // }
                //    }

                JObject Parameters = JObject.Parse(@"" + param);
                MedullusSendGridEmailLib.MedullusSendGridEmailLib sendMail = new MedullusSendGridEmailLib.MedullusSendGridEmailLib();
                var result = sendMail.SendStaticEmailBySendGrid(Parameters);
                return result.ToLower() == "success" ? true : false;
                // return false;
                // }
                // return false;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public bool SendAll(string Subject, string Body, string[] MailTo)
        {
            // throw new NotImplementedException();
            try
            {
                var param = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    emailHeader = new
                    {
                        EmailSenderId = ConfigurationManager.AppSettings["SendEmailUserName"],
                        EmailSubject = Subject,
                        EmailBody = Body,
                        ClientId = ConfigurationManager.AppSettings["SendEmailClientId"],
                        EmailSenderPasswd = ConfigurationManager.AppSettings["SendEmailPassword"]
                    },
                    emailDetails = MailTo.Select(x => new { EmailReceiverId = x })
                });
                HttpContent ContentPost = new StringContent(param, Encoding.UTF8, "application/json");
                using (HttpClient Client = new HttpClient())
                {
                    // client.BaseAddress = new Uri(apiUrl);
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var Response = Client.PostAsync(string.Format("{0}/{1}", ConfigurationManager.AppSettings["SendEmailApiUrl"],
                       ConfigurationManager.AppSettings["SendEmailMethod"]
                       ), ContentPost).Result;
                    if (Response.IsSuccessStatusCode)
                    {
                        var data = Response.Content.ReadAsStringAsync().ToString();
                        // var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                        if (data == "Success")
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}