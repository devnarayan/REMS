using REMS.Data;
using REMS.Data.Access;
using REMS.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace REMS.Web.App_Helpers
{
    public class PrintReceipt
    {
        DataFunctions obj = new DataFunctions();
        public string ReceiptDetail(ReceiptModel model)
        {
            try
            {
                DataTable dt = new DataTable();
                string stStr = string.Empty;
                string emailid = model.ToEmailID;
                Hashtable ht = new Hashtable();
                ht.Add("TransactionID", model.TransactionID);
                string l_FileName = "";
                dt = obj.GetDataTableFromProcedure("Get_Payment", ht);
                if (dt.Rows.Count > 0)
                {
                    string PaidDate = string.Empty;
                    string ReceiptNo = string.Empty;
                    string ClientName = string.Empty;
                    string CoApplicantName = string.Empty;
                    string CoAppAddress = string.Empty;
                    string GName = string.Empty;
                    string Address = string.Empty;
                    string AmtInWord = string.Empty;
                    string FlatNo = string.Empty;
                    string Location = string.Empty;
                    string DocumentDesc = string.Empty;
                    string Amount = string.Empty;
                    string CoFName = string.Empty;
                    string CoParentName = string.Empty;
                    //  string emailid = string.Empty;
                    stStr += "<table cellpadding='0' cellspacing='0' border='0'>";
                    for (Int32 i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        PaidDate = dt.Rows[i]["PaymentDate"].ToString();
                        ReceiptNo = dt.Rows[i]["PaymentNo"].ToString();
                        ClientName = dt.Rows[i]["AppTitle"].ToString() + " " + dt.Rows[i]["FName"].ToString() + " " + dt.Rows[i]["MName"].ToString() + " " + dt.Rows[i]["LName"].ToString();
                        GName = dt.Rows[i]["Title"].ToString() + " " + dt.Rows[i]["PName"].ToString();
                        Address = dt.Rows[i]["Address"].ToString();
                        //  CoApplicantName = dt.Rows[i]["CoApplicantName"].ToString().Trim();
                        // CoAppAddress = dt.Rows[i]["CoAppAddress"].ToString().Trim();
                        AmtInWord = dt.Rows[i]["AmtRcvdinWords"].ToString();
                        FlatNo = dt.Rows[i]["FlatName"].ToString();
                        Location = dt.Rows[i]["CompanyAddress"].ToString();
                        DocumentDesc = dt.Rows[i]["PaymentMode"].ToString();
                        Amount = dt.Rows[i]["Amount"].ToString();
                        // CoFName = dt.Rows[i]["CoFName"].ToString().Trim();
                        //  CoParentName = dt.Rows[i]["CoParentName"].ToString().Trim();
                        //  emailid = dt.Rows[0]["EmailID"].ToString();
                        if (emailid != "")
                        {
                            stStr += "<table width='620px' height='875px' cellpadding='0' cellspacing='0' border='0'";
                            stStr += "<tr><td>";
                            stStr += "<table cellpadding='0' cellspacing='0' border='0'>";
                            stStr += "<tr><td colspan='2'>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2'>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2'>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' align='ceneter'><h1>Payment Receipt</h1></td></tr>";
                            stStr += "<tr><td class='label'><b>No:</b>" + ReceiptNo + "</td><td class='label' align='right'><b>Date:</b>" + PaidDate + "</td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' class='label'>Received with thanks from <b>" + ClientName + "</b></td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' class='label'>" + GName + "  ";
                            //if (CoFName != "")
                            //{
                            //    stStr += "And";
                            //    stStr += "" + CoApplicantName + "";
                            //    //stStr += "<tr><td>&nbsp;</td></tr>";
                            //    //stStr += "<tr><td colspan='2' class='label'>R/O" + CoAppAddress + "</td></tr>";
                            //    //stStr += "<tr><td>&nbsp;</td></tr>";
                            //}
                            stStr += "</td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' class='label'>R/O " + Address + "</td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            //CoApplicant Detail

                            /////End
                            stStr += "<tr><td colspan='2' class='label'>A Sum of Rs.<b>" + AmtInWord + "</b></td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' class='label'>against the House No/FlatNo <b>" + FlatNo + "</b> in situated at</td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' class='label'>" + Location + ".</td></tr>";
                            stStr += "<tr><td>&nbsp;</td></tr>";
                            stStr += "<tr><td colspan='2' class='label'>in form of <b>" + DocumentDesc + "</b></td></tr>";
                            stStr += "<tr><td>&nbsp; <br/> <br/><br/> <br/></td></tr>";

                            stStr += "<tr><td>&nbsp; <br/> <br/><br/> <br/></td></tr>";
                            stStr += "<tr><td class='label'>Rs. <b>" + Amount + "</b></td><td class='label'><b>Authorized Signatory</b></td></tr>";
                            stStr += "</table>";
                        }

                    }

                    stStr += "</td></tr>";
                    stStr += "</table>";

                    string pref = string.Empty;


                    string fileName = dt.Rows[0]["PaymentNo"].ToString();

                    fileName = @"~/PaymentReceipt/PaymentReceipt_" + fileName + ".doc";
                    FileStream myfile = new FileStream(HostingEnvironment.MapPath(fileName), FileMode.OpenOrCreate, FileAccess.Write);
                    myfile.Close();
                    myfile.Dispose();
                    File.WriteAllText(HostingEnvironment.MapPath(fileName), stStr.ToString());


                    return fileName;
                }
                return "";
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
                return "";
            }
        }

        public string GenerateReceipt(ReceiptModel model)
        {
            DataTable dt = new DataTable();
            string stStr = string.Empty;
            Hashtable ht = new Hashtable();
            ht.Add("TransactionID", model.TransactionID);
            string l_FileName = "";

            REMSDBEntities context = new REMSDBEntities();
            int tids = model.TransactionID;
            var pay = context.Payments.Where(p => p.TransactionID == tids).FirstOrDefault();
            //var pop = (from sale in context.tblSSaleFlats join p in context.tblSProperties on sale.PropertyID equals p.PID where sale.SaleID == pay.SaleID select new { PropertyName=p.PName, PID=p.PID, PropertyAddress=p.Address+ " "+p.Village+" "+p.Tehsil }).AsEnumerable();
            var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
            int? pid = context.SaleFlats.Where(s => s.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
            var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();
            string bc = "", cd = "";
            if (pay.BankClearanceDate == null) bc = "";
            else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
            if (pay.ChequeDate == null) cd = "";
            else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");

            string htmlText = ReceiptBody1();
            htmlText = htmlText.Replace("<%FName%>", pay.CustomerName).Replace("<%FAddress%>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode).Replace("<%CName%>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName).Replace("<%PropertyUnitAddress%>", pay.FlatName).Replace("<%PaymentDetails%>", pay.PaymentMode + " " + pay.ChequeNo + " " + pay.BankName + " " + pay.BankBranch + " " + pay.ChequeDate).Replace("<%InstallmentNo%>", pay.InstallmentNo).Replace("<%Amount%>", pay.Amount.Value.ToString()).Replace("<%AmountWord%>", pay.AmtRcvdinWords).Replace("<%PaymentNo%>", pay.PaymentNo).Replace("<%PropertyAddress%>", Property.Address).Replace("<%PropertyName%>", Property.PName).Replace("<%PropertyLocation%>", Property.Location).Replace("<%CompanyName%>", Property.CompanyName).Replace("<%SignAdmin%>", Property.AuthoritySign);
            stStr += htmlText;
            string fileName = @"~/PaymentReceipt/Receipt(" + Property.ReceiptPrefix + "_" + pay.FlatName + "_" + pay.PaymentNo + ").doc";
            FileStream myfile = new FileStream(HostingEnvironment.MapPath(fileName), FileMode.OpenOrCreate, FileAccess.Write);
            myfile.Close();
            myfile.Dispose();
            File.WriteAllText(HostingEnvironment.MapPath(fileName), stStr.ToString());
            return fileName;
            //TestPDF.HtmlToPdfBuilder h2p = new TestPDF.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
            //return   h2p.HTMLToPdf(htmlText, model.TransactionID.ToString());
        }
        public string GenerateReceiptOtherPayment(ReceiptModel model, string payfor)
        {
            DataTable dt = new DataTable();
            string stStr = string.Empty;
            Hashtable ht = new Hashtable();
            ht.Add("TransactionID", model.TransactionID);
            string l_FileName = "";


            REMSDBEntities context = new REMSDBEntities();
            int tids = model.TransactionID;
            var pay = context.PaymentOthers.Where(p => p.TransactionID == tids).FirstOrDefault();
            var cust = context.Customers.Where(c => c.SaleID == pay.SaleID && c.SaleStatus == true).FirstOrDefault();
            int? pid = context.SaleFlats.Where(s => s.SaleID == pay.SaleID).FirstOrDefault().ProjectID;
            var Property = context.Projects.Where(p => p.ProjectID == pid).FirstOrDefault();
            string bc = "", cd = "";
            if (pay.BankClearanceDate == null) bc = "";
            else bc = pay.BankClearanceDate.Value.ToString("dd/MM/yyyy");
            if (pay.ChequeDate == null) cd = "";
            else cd = pay.ChequeDate.Value.ToString("dd/MM/yyyy");

            string htmlText = ReceiptBody1();
            htmlText = htmlText.Replace("<%FName%>", pay.CustomerName).Replace("<%FAddress%>", cust.Address1 + " " + cust.Address2 + " " + cust.City + " " + cust.State + " " + cust.PinCode).Replace("<%CName%>", cust.CoAppTitle + " " + cust.CoFName + " " + cust.CoMName + " " + cust.CoLName).Replace("<%PropertyUnitAddress%>", pay.FlatName).Replace("<%PaymentDetails%>", pay.PaymentMode + " " + pay.ChequeNo + " " + pay.BankName + " " + pay.BankBranch + " " + pay.ChequeDate).Replace("<%InstallmentNo%>", payfor).Replace("<%Amount%>", pay.Amount.Value.ToString()).Replace("<%AmountWord%>", pay.AmtRcvdinWords).Replace("<%PaymentNo%>", pay.PaymentNo).Replace("<%PropertyAddress%>", Property.Address).Replace("<%PropertyName%>", Property.PName).Replace("<%PropertyLocation%>", Property.Location);
            stStr += htmlText;
            string fileName = @"~/PaymentReceipt/Receipt(" + Property.ReceiptPrefix + "_" + pay.FlatName + "_" + payfor + "_" + pay.PaymentNo + ").doc";
            FileStream myfile = new FileStream(HostingEnvironment.MapPath(fileName), FileMode.OpenOrCreate, FileAccess.Write);
            myfile.Close();
            myfile.Dispose();
            File.WriteAllText(HostingEnvironment.MapPath(fileName), stStr.ToString());
            return fileName;
        }

        public void SendMailfinal(string From, string Subject, string Body, string To, string FileName)
        {
            try
            {
                MailMessage email = new MailMessage();
                email.From = new MailAddress(ConfigurationManager.AppSettings["email"], Subject);
                email.Subject = Subject;
                email.IsBodyHtml = true;

                System.Net.Mail.Attachment MyAttachment = null;
                MyAttachment = new System.Net.Mail.Attachment(HostingEnvironment.MapPath(FileName));
                email.Attachments.Add(MyAttachment);
                DateTime dt = DateTime.Now;

                email.Body =  Convert.ToString(Body);
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
                email.To.Add(To);
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Helper h = new Helper();
                h.LogException(ex);
            }
        }
        private string ReceiptBody()
        {
            String htmlText = @"   <div><br />
        <div style='text-align: center'>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
           <br/>
            <h3>RECEIPT: <%PaymentNo%> </h3>
            <br />
        </div>

        Received with thanks from <br />

        First Allottee: <%FName%> <br />
        <%FAddress%>
        <br />

        Co Allottee(s): &nbsp; <%CName%> <br />
        <p>Payment in the respect to Property : <%PropertyUnitAddress%> </p>

        <p> Vide <%PaymentDetails%> </p><br />
        <table style='width:100%;'>
            <tr style='border:1px solid black'><td>Installment Description</td><td>Amount (Rs)</td>
            <tr>
            <tr style='border:1px solid black'><td> <%InstallmentNo%> </td><td> <%Amount%> </td></tr>
            <tr><td> </td> <td>&nbsp;  </td></tr>
        </table>
        Rupees <%AmountWord%><b style='text-align:right; float:right;'>Total Amount: <%Amount%> </b>
        <br />
       <p>Property at: <%PropertyAddress%> , <b> <%PropertyName%> </b><br/>
Location: <%PropertyLocation%>      
</p>
        <table>
            <tr>
                <td>
                    <ul>
                        <li>Receipt is valid subject to realisation of cheque.</li>
                       
                    </ul>
                </td>
                <td>
                   
                </td>
            </tr>
        </table>
 <p><b>for SBP GROUPS. </b></p>
        <br />
        <br />
        <br />
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <br />
                    (Prepared By)<br />
                    Sunita.d
                </td>
                <td><p><b> </b></p> </td>
            </tr>
        </table>
        <br />

        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>
               
";
            return htmlText;
        }
        private string ReceiptBody1()
        {
            String htmlText = @"   <!DOCTYPE html>

<html>
<head>
    <meta name='viewport' content='width=device-width' />
    <title>Backup Demand Print</title>

    

    <style>
      .mytbls  td{
            border-bottom:1px solid gray;
            border-left:1px solid gray;
        }
    </style>
</head>
<body >
    <div id='ReceiptPritable' >
        
        <div >
           
            <div>
                <div style='text-align: center'>
                    <h2></h2>
                    <p><b></b></p>
                    <p></p>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <h3>RECEIPT: <%PaymentNo%> </h3>
                </div>

                Received with thanks from
                <br />
                <table>
                    <tr>
                        <td>
                            First Allottee:
                        </td>
                        <td> <%FName%>  </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td> <%CName%> </td>
                    </tr>
                </table>

                Co Allottee(s): &nbsp;  <%CName%> 
                <br />
                <div class='mytbls'>
                    <table style='border:2px solid gray; width:100%;'>
                        <tr>
                            <td>
                                Property Name:
                            </td>
                            <td>
                               <%PropertyName%> 
                            </td>
                        </tr>
                        <tr style='border: 1px solid black'>
                            <td>Installment</td>
                            <td> <%InstallmentNo%>   </td>
                        <tr>
 <tr style='border: 1px solid black'>
                            <td>Details</td>
                            <td> <%PaymentDetails%>   </td>
                        <tr>
                        
                        <tr style='border: 1px solid black'>
                            <td>Amount (Rs)</td>
                            <td> <%Amount%>  </td>
                        </tr>
                        <tr>
                            <td>  </td>
                            <td><%AmountWord%>   </td>
                        </tr>
                    </table>
                </div>
                <p>
                    Property at:  <%PropertyAddress%> , <b><%PropertyName%>  </b>
                    <br />
                    Location: <%PropertyLocation%> 

                </p>
                <table>
                    <tr>
                        <td>
                            <ul>
                                <li>Receipt is valid subject to realisation of cheque.</li>
                            </ul>
                        </td>
                        <td></td>
                    </tr>
                </table>
                <p><b>for <%CompanyName%> </b></p>
                <table>
                    <tr>
                        <td>
                            <b>Authorised Signatury </b>
                            <br />
                            (Prepared By)<br />
                           <%SignAdmin%>
                        </td>
                        <td>
                            <p style='text-align: right;'></p>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />

            </div>
        </div>
    </div>
  
</body>
</html>






                         
          



                    

";


            //     RECEIPT: <%PaymentNo%> 




            //Received with thanks from 

            //First Allottee: <%FName%> 
            //<%FAddress%>

            //Co Allottee(s):  <%CName%> 
            //Payment in the respect to Property : <%PropertyUnitAddress%> 

            //Vide <%PaymentDetails%> 

            //Installment Description                          Amount (Rs)
            //<%InstallmentNo%>                                <%Amount%>

            //Rupees <%AmountWord%>                            Total Amount: <%Amount%> 

            //Property at: <%PropertyAddress%> ,  <%PropertyName%> 
            //Location: <%PropertyLocation%>      


            //Receipt is valid subject to realisation of cheque.


            //for SBP GROUPS. 


            //(Prepared By)
            //Sunita.d


            return htmlText;
        }
    }
}