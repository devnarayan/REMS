using AutoMapper;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Custo
{
    public interface IInfoServices
    {
        CustomerModel GetCustomerBySaleID(string saleId);
        CustomerModel GetCustomerBySaleIdBy(string saleId);
        CustomerModel GetCustomerByCustId(string saleId);
        FlatSaleModel GetFlatSaleBySaleId(string saleId);
        List<PaymentModel> GetPaymentBySaleID(string saleId);
        Flat GetFlatInfoByFlatId(string flatId);
    }
    public class InfoServices
    {
        public CustomerModel GetCustomerBySaleID(string saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(saleId);
                var cust = context.Customers.Where(c => c.SaleID == sid && c.SaleStatus.Value == true).FirstOrDefault();
                Mapper.CreateMap<REMS.Data.Customer, CustomerModel>();
                var model = Mapper.Map<REMS.Data.Customer, CustomerModel>(cust);
                return model;
            }
        }
        public CustomerModel GetCustomerBySaleIdBy(string saleId)
        {

            int CID = 0;
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(saleId);
                var Scust = (from sale in context.SaleFlats
                             join ft in context.Flats on sale.FlatID equals ft.FlatID
                             join cr in context.Customers on sale.SaleID equals cr.SaleID
                             where sale.SaleID == sid && cr.Status == "1"
                             select new { cr = cr });
                foreach (var v in Scust)
                {
                    CID = Convert.ToInt32(v.cr.CustomerID);
                }
                var cust = context.Customers.Where(c => c.CustomerID == CID && c.SaleStatus.Value == true).FirstOrDefault();
                Mapper.CreateMap<REMS.Data.Customer, CustomerModel>();
                var model = Mapper.Map<REMS.Data.Customer, CustomerModel>(cust);
                return model;
            }
        }
        public CustomerModel GetCustomerByCustId(string saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(saleId);
                var cust = context.Customers.Where(c => c.CustomerID == sid && c.SaleStatus.Value == true).FirstOrDefault();
                Mapper.CreateMap<REMS.Data.Customer, CustomerModel>();
                var model = Mapper.Map<REMS.Data.Customer, CustomerModel>(cust);
                return (model);
            }
        }
        public FlatSaleModel GetFlatSaleBySaleId(string saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {

                int sid = Convert.ToInt32(saleId);
                var Sale = context.SaleFlats.Where(c => c.SaleID == sid).FirstOrDefault();
                Mapper.CreateMap<SaleFlat, FlatSaleModel>();
                var model = Mapper.Map<SaleFlat, FlatSaleModel>(Sale);
                if (model != null)
                {
                    if (model.SaleDate.Value != null)
                        model.SaleDateSt = model.SaleDate.Value.ToString("dd/MM/yyyy");
                }
                if (model != null)
                {
                    if (model.BookingDate != null)
                        model.BookingDateSt = model.BookingDate.Value.ToString("dd/MM/yyyy");
                }
                return model;
            }

        }
        public List<PaymentModel> GetPaymentBySaleID(string saleId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int sid = Convert.ToInt32(saleId);
                var payment = (from p in context.Payments join c in context.Customers on p.CustomerID equals c.CustomerID where p.SaleID == sid select new { Pay = p, CustomerName = c.AppTitle + " " + c.FName + " " + c.LName + " " + c.MName });
                List<PaymentModel> model1 = new List<PaymentModel>();
                foreach (var v in payment)
                {
                    string bdate = "";
                    if (v.Pay.PaymentDate != null)
                        bdate = Convert.ToDateTime(v.Pay.PaymentDate).ToString("dd/MM/yyyy");
                    string cdate = "";
                    if (v.Pay.ChequeDate != null)
                        cdate = Convert.ToDateTime(v.Pay.ChequeDate).ToString("dd/MM/yyyy");
                    model1.Add(new PaymentModel { ChequeDateSt = cdate, PaymentDateSt = bdate, Activity = v.Pay.Activity, BankBranch = v.Pay.BankBranch, AmtRcvdinWords = v.Pay.AmtRcvdinWords, BankCharges = v.Pay.BankCharges, BankClearanceDate = v.Pay.BankClearanceDate, BankName = v.Pay.BankName, ChequeDate = v.Pay.ChequeDate, ChequeNo = v.Pay.ChequeNo, ClearanceCharge = v.Pay.ClearanceCharge, PaymentStatus = v.Pay.PaymentStatus, CreatedBy = v.Pay.CreatedBy, InstallmentNo = v.Pay.InstallmentNo, CustomerID = v.Pay.CustomerID, TransactionID = v.Pay.TransactionID, FlatName = v.Pay.FlatName, CustomerName = v.CustomerName, PaymentDate = v.Pay.PaymentDate, PaymentMode = v.Pay.PaymentMode, Remarks = v.Pay.Remarks, SaleID = v.Pay.SaleID, Amount = v.Pay.Amount, PaymentNo = v.Pay.PaymentNo, PaymentID = v.Pay.PaymentID });
                }
                return model1;
            }
        }
        public Flat GetFlatInfoByFlatId(string flatId)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int fId = Convert.ToInt32(flatId);
                var model = context.Flats.Where(p => p.FlatID == fId).FirstOrDefault();
                return model;
            }
        }
        //public string ExportSummary(string shtml)
        //{

        //    shtml = "<html lang='en-us'><head>" +



        //        "</head><body>" + shtml + "</body></html>";



        //    //      " <style>" +
        //    //" table {" +
        //    //     "border: 2px solid gray;" +
        //    //     "width: 100%;" +
        //    // "}" +

        //    //     "table thead {" +
        //    //         "border: 1px solid gray;" +
        //    //         "background-color: rgb(59, 57, 61);" +
        //    //         "padding: 0;" +
        //    //         "margin: 0;" +
        //    //         "color: rgb(247, 13, 13);" +
        //    //     "}" +

        //    //     "table tbody tr {" +
        //    //         "border-bottom: 1px solid;" +
        //    //         "border-color: gray;" +
        //    //     "}" +

        //    // "td {" +
        //    //     "border-bottom: 1px solid gray;" +
        //    //     "border-left: 1px solid gray;" +
        //    // "}" +

        //    // "tr {" +
        //    //     "border-bottom: 1px solid red;" +
        //    // "}</style>";


        //    string path = HTMLToPdfExportBackupReceiptExcel(shtml, "SummaryExport");

        //    string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Temp" + path;


        //    //WebClient webClient = new WebClient();

        //    //webClient.DownloadFileAsync(new Uri("http://localhost:62421/pdf/temp/SummaryExport1301858907.pdf"), @"" + filePath + "");




        //    //Response.ClearHeaders();
        //    //Response.ContentType = "application/pdf";
        //    //Response.AddHeader("Content-Disposition", "attachment; filename=pdffile.pdf");
        //    //Response.TransmitFile(filePath);
        //    //Response.End();
        //    //   string fileName = path;
        //    //   string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Temp\\";
        //    // Response.Clear();
        //    //Response.ContentType = "application/pdf";
        //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    // Response.WriteFile(filePath + fileName);
        //    //Response.End();


        //    //Response.Clear();
        //    //Response.AddHeader("Content-Disposition", "attachment; filename=Report.xls");

        //    ////Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    //Response.ContentType = "application/vnd.ms-excel";



        //    //System.IO.StringWriter writer = new System.IO.StringWriter();

        //    //System.Web.UI.HtmlTextWriter html = new System.Web.UI.HtmlTextWriter(writer);
        //    //string style = @"<style> .textmode { mso-number-format:\@; } </style>";

        //    //Response.Write(style);


        //    //Response.Write(shtml.ToString());

        //    //Response.End();


        //    //WebClient webClient = new WebClient();
        //    //webClient.DownloadFile(filePath, path);
        //    //webClient.Dispose(); //I added this l

        //    //Response.ContentType = "Application/pdf";
        //    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + path + "");
        //    //Response.TransmitFile(Server.MapPath("~/PDF/Temp/" + path + ""));
        //    //Response.End();


        //    //shtml = shtml.Replace("\\n", " ");

        //    //System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/" + path + ""), shtml);
        //    return path;
        //}
        //public string SummarySendMail(string shtml, string email)
        //{
        //    shtml = shtml.Replace("\\n", " ");
        //    System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/SummaryExport.doc"), shtml);
        //    SendMail sm = new SendMail();
        //    sm.BackupReceiptMailDataFile("Payment Summary from SBP Groups", "", email, "SummaryExport.doc");
        //    return "Yes";
        //}
        //public string HTMLToPdfExportBackupReceiptExcel(string HTML, string FilePath)
        //{
        //    FilePath = FilePath.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString();
        //    Document document = new Document();

        //    try
        //    {
        //        PdfWriter.GetInstance(document, new FileStream(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Temp\\" + FilePath.ToString() + ".pdf", FileMode.Create));
        //    }
        //    catch (IOException ex)
        //    {
        //        PdfWriter.GetInstance(document, new FileStream(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\Temp\\" + FilePath.ToString() + ".pdf", FileMode.Create));
        //    }
        //    document.Open();


        //    iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
        //    //StyleSheet styles = new StyleSheet();
        //    //styles.LoadStyle(".table12", "border", "2");
        //    styles.LoadTagStyle("#headerdiv", "border", "2");
        //    //styles.LoadStyle("table", "border", "2px");

        //    iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
        //    hw.SetStyleSheet(styles);
        //    hw.Parse(new StringReader(HTML));
        //    document.Close();
        //    document.Dispose();
        //    return FilePath + ".pdf";
        //}
    }
}
