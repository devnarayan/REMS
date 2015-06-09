using REMS.Data.Access;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace REMS.Web.App_Helpers
{
    public class SendMail
    {
        public void BackupReceiptMail(string Subject, string Body, string To, string[] tids)
        {
            try
            {
                System.Net.Mail.MailMessage MyMailMessage = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["tomail"], To);
                MyMailMessage.IsBodyHtml = true;
                MyMailMessage.Subject = Subject;
                MyMailMessage.Body = Convert.ToString(Body);
                System.Net.Mail.Attachment MyAttachment = null;
                foreach (string s in tids)
                {
                    MyAttachment = new System.Net.Mail.Attachment(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Temp\\" + s.ToString() + ".pdf");
                    MyMailMessage.Attachments.Add(MyAttachment);
                }
                 string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                 System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(host,port);
                mailClient.EnableSsl = true;
                 string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential(from_email, password);
                mailClient.UseDefaultCredentials = false;

                mailClient.Credentials = mailAuthentication;
                mailClient.Send(MyMailMessage);
                MyMailMessage.Dispose();

            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
        }
        public void BackupReceiptMailDataFile(string Subject, string Body, string To, string FileName)
        {
            try
            {
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["tomail"], ConfigurationManager.AppSettings["SenderName"]);
                email.IsBodyHtml=true;
                email.Subject = Subject;
                email.Body = Convert.ToString(Body);
                Attachment attachment = new System.Net.Mail.Attachment(HostingEnvironment.MapPath("~/PDF/Temp/" + FileName));
                email.Attachments.Add(attachment);
                string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                SmtpClient smtp = new SmtpClient(host);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential nc = new NetworkCredential(from_email, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                email.IsBodyHtml = true;
                email.To.Add(To);
                smtp.Send(email);
                email.Dispose();
               

               // System.Net.Mail.MailMessage MyMailMessage = new System.Net.Mail.MailMessage("balkrishnap@synoris.com", To);
               // MyMailMessage.IsBodyHtml = true;
               // MyMailMessage.Subject = Subject;
               // MyMailMessage.Body = Convert.ToString(Body);
               // System.Net.Mail.Attachment MyAttachment = null;
               // MyAttachment = new System.Net.Mail.Attachment(HostingEnvironment.MapPath("~/PDF/Temp/" + FileName));
               // MyMailMessage.Attachments.Add(MyAttachment);
               // // MyMailMessage.Attachments.Add(MyAttachment);
               //// System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("mail.sbpgroups.in");
               // System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("smtp.gmail.com",587);
               // mailClient.EnableSsl = true;
               //  System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential("balkrishnap@synoris.com", "SynoBalkrishna");
               // // System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential("info@sbpgroups.in", "sbp123");

               //// mailClient.Port = 25;
               // mailClient.UseDefaultCredentials = false;

               // mailClient.Credentials = mailAuthentication;
               // mailClient.Send(MyMailMessage);
               // MyMailMessage.Dispose();

            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
        }
        public static string AgreementMail(string emailid, string name, string url)
        {
            try
            {
                //u.GetPassword();
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["tomail"], ConfigurationManager.AppSettings["SenderName"]);

                email.Subject = "Porperty Agreement copy‏";
                email.IsBodyHtml = true;
                StringBuilder Message = new StringBuilder();
                string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                SmtpClient smtp = new SmtpClient(host);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential nc = new NetworkCredential(from_email, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                email.IsBodyHtml = true;


                email.To.Add(emailid);
                Attachment attachment = new System.Net.Mail.Attachment(HostingEnvironment.MapPath("~/" + url));
                email.Attachments.Add(attachment);

                smtp.Send(email);
                return "OK";
            }
            catch (Exception ex)
            {
                Helper helper = new Helper();
                helper.LogException(ex);
                return "Error";
            }
        }

        public void MailResetPassword(string body, string emailid)
        {
            try
            {
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["tomail"], "REMS Mail System");

                email.Subject = "Reset Password";
                email.IsBodyHtml = true;

                DateTime dt = DateTime.Now;

                email.Body = Convert.ToString(body);
                string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                SmtpClient smtp = new SmtpClient(host, port);
                smtp.UseDefaultCredentials = false;
                NetworkCredential nc = new NetworkCredential(from_email, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                email.IsBodyHtml = true;

                email.To.Add(emailid);

                // Attachment attachment = new Attachment(Path.Combine(HostingEnvironment.MapPath("~/Pdfdocs"), model.PdfUrl));
                //  email.Attachments.Add(attachment);
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
        }

        public static string SendContactUsMailFunding(string emailid, string name)
        {
            try
            {
                //u.GetPassword();
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["tomail"]);

                email.Subject = "Thanks for contact us: [ Secured Investment Corp. ]‏";
                email.IsBodyHtml = true;
                StringBuilder Message = new StringBuilder();
                string path = HostingEnvironment.MapPath(@"~\MailTemplates\ContactFunding.html");
                using (StreamReader reader = new StreamReader(path))
                {
                    Message = new StringBuilder(reader.ReadToEnd());
                }

                DateTime dt = DateTime.Now;
                Message.Replace("Name_Token", name);
                Message.Replace("Domain_Token", ConfigurationManager.AppSettings["domain"].ToString());

                email.Body = Convert.ToString(Message);
                string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                SmtpClient smtp = new SmtpClient(host, port);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential nc = new NetworkCredential(from_email, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                email.IsBodyHtml = true;


                email.To.Add(emailid);
                //Attachment attachment = new Attachment(Path.Combine(HostingEnvironment.MapPath("~/Pdfdocs"), model.PdfUrl));
                //email.Attachments.Add(attachment);

                smtp.Send(email);
                return "OK";
            }
            catch (Exception ex)
            {
                Helper helper = new Helper();
                helper.LogException(ex);
                return "Error";
            }
        }
        public static string SendContactUsMail(string emailid, string name)
        {
            try
            {
                //u.GetPassword();
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["tomail"]);

                email.Subject = "Thanks for contacting us: [ Secured Investment Corp. ]‏";
                email.IsBodyHtml = true;
                StringBuilder Message = new StringBuilder();
                string path = HostingEnvironment.MapPath(@"~\MailTemplates\ContactForm.html");
                using (StreamReader reader = new StreamReader(path))
                {
                    Message = new StringBuilder(reader.ReadToEnd());
                }

                DateTime dt = DateTime.Now;
                Message.Replace("Name_Token", name);
                Message.Replace("Domain_Token", ConfigurationManager.AppSettings["domain"].ToString());

                email.Body = Convert.ToString(Message);
                string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                SmtpClient smtp = new SmtpClient(host, port);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential nc = new NetworkCredential(from_email, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                email.IsBodyHtml = true;


                email.To.Add(emailid);
                //Attachment attachment = new Attachment(Path.Combine(HostingEnvironment.MapPath("~/Pdfdocs"), model.PdfUrl));
                //email.Attachments.Add(attachment);

                smtp.Send(email);
                return "OK";
            }
            catch (Exception ex)
            {
                Helper helper = new Helper();
                helper.LogException(ex);
                return "Error";
            }
        }
        public static string JobApplication(string designation, string name, string applicationurl)
        {
            try
            {
                //u.GetPassword();
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["tomail"]);

                email.Subject = "New Job Application from WebSite.‏";
                email.IsBodyHtml = true;
                StringBuilder Message = new StringBuilder();
                string path = HostingEnvironment.MapPath(@"~\MailTemplates\ApplyNow.html");
                using (StreamReader reader = new StreamReader(path))
                {
                    Message = new StringBuilder(reader.ReadToEnd());
                }

                DateTime dt = DateTime.Now;
                Message.Replace("Name_Token", name);
                Message.Replace("Designation_Token", designation);
                Message.Replace("Application_Token", Path.Combine(HostingEnvironment.MapPath("~/JobApply"), applicationurl));

                email.Body = Convert.ToString(Message);
                string from_email = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];
                string host = ConfigurationManager.AppSettings["host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                SmtpClient smtp = new SmtpClient(host, port);
                smtp.UseDefaultCredentials = false;
                NetworkCredential nc = new NetworkCredential(from_email, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                email.IsBodyHtml = true;
                email.To.Add(ConfigurationManager.AppSettings["tomail"]);
                Attachment attachment = new Attachment(Path.Combine(HostingEnvironment.MapPath("~/JobApply"), applicationurl));
                email.Attachments.Add(attachment);
                smtp.Send(email);
                return "OK";
            }
            catch (Exception ex)
            {
                Helper helper = new Helper();
                helper.LogException(ex);
                return "Error";
            }
        }


        public void SendForgetPasswords(string Subject, string Body, string To)
        {
            try
            {
                System.Net.Mail.MailMessage MyMailMessage = new System.Net.Mail.MailMessage("info@sbpgroups.in", To);
                MyMailMessage.IsBodyHtml = true;
                MyMailMessage.Subject = Subject;
                MyMailMessage.Body = Convert.ToString(Body);
                // System.Net.Mail.Attachment MyAttachment = null;
                //MyAttachment = new System.Net.Mail.Attachment(HostingEnvironment.MapPath("~/PDF/Temp/" + FileName));
                //MyMailMessage.Attachments.Add(MyAttachment);
                // MyMailMessage.Attachments.Add(MyAttachment);
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("mail.sbpgroups.in");
                // System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("smtp.gmail.com",587);
                mailClient.EnableSsl = false;
                // System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential("lalitgulati02@gmail.com", "");
                System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential("info@sbpgroups.in", "sbp123");

                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;

                mailClient.Credentials = mailAuthentication;
                mailClient.Send(MyMailMessage);
                MyMailMessage.Dispose();

            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
        }

    }
}