using REMS.Data.DataModel;
using REMS.Data.Access.Custo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using REMS.Data.Access.Sale;

namespace REMS.Web.Areas.Customer.Controllers
{
    public class ManageCustomerController : Controller
    {
        public DataFunctions obj;
        public CustomerService custService;
        public SaleFlatService saleflatservcie;
        public ManageCustomerController(){
            custService = new CustomerService();
            saleflatservcie = new SaleFlatService();
            obj = new DataFunctions();
        }
        // GET: Customer/ManageCustomer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditCustomerFlatID(int id)
        {
           var model= saleflatservcie.GetSaleFlatByFlatID(id);
           return RedirectToAction("EditCustomer", "ManageCustomer", new { area = "Customer", id = model.SaleID });
        }
        public ActionResult EditCustomer(string Id)
        {

            ViewBag.ID = Id;
            return View();
        }

        #region Customer Services
        public string custDetail(string saleId)
        {
            CustomerModel custDetail = new CustomerModel();
            DataTable dtCust;
            dtCust = obj.GetDataTable("select * from Customer where SaleID=" + saleId);
            if (dtCust.Rows.Count > 0)
            {
                custDetail.CustomerID = Convert.ToInt32(dtCust.Rows[0]["CustomerID"]);
                custDetail.AppTitle = Convert.ToString(dtCust.Rows[0]["AppTitle"]);
                custDetail.FName = Convert.ToString(dtCust.Rows[0]["FName"]);
                custDetail.MName = Convert.ToString(dtCust.Rows[0]["MName"]);
                custDetail.LName = Convert.ToString(dtCust.Rows[0]["LName"]);
                custDetail.PName = Convert.ToString(dtCust.Rows[0]["PName"]);
                custDetail.Address1 = Convert.ToString(dtCust.Rows[0]["Address1"]);
                custDetail.Address2 = Convert.ToString(dtCust.Rows[0]["Address2"]);
                custDetail.City = Convert.ToString(dtCust.Rows[0]["City"]);
                custDetail.State = Convert.ToString(dtCust.Rows[0]["State"]);
                custDetail.Country = Convert.ToString(dtCust.Rows[0]["Country"]);
                custDetail.MobileNo = Convert.ToString(dtCust.Rows[0]["MobileNo"]);
                custDetail.PAN = Convert.ToString(dtCust.Rows[0]["PAN"]);
                custDetail.AlternateMobile = Convert.ToString(dtCust.Rows[0]["AlternateMobile"]);
                string dob = dtCust.Rows[0]["DateOfBirth"].ToString();
                if (dob == null || dob == "") custDetail.DateOfBirthst = "";
                else
                    custDetail.DateOfBirthst = Convert.ToDateTime(dtCust.Rows[0]["DateOfBirth"]).ToString("dd/MM/yyyy");
                custDetail.EmailID = Convert.ToString(dtCust.Rows[0]["EmailID"]);
                custDetail.LandLine = Convert.ToString(dtCust.Rows[0]["LandLine"]);
                custDetail.AlternateEmail = Convert.ToString(dtCust.Rows[0]["AlternateEmail"]);
                custDetail.CoAppTitle = Convert.ToString(dtCust.Rows[0]["CoAppTitle"]);
                custDetail.CoFName = Convert.ToString(dtCust.Rows[0]["CoFName"]);
                custDetail.CoMName = Convert.ToString(dtCust.Rows[0]["CoMName"]);
                custDetail.CoLName = Convert.ToString(dtCust.Rows[0]["CoLName"]);
                custDetail.CoTitle = Convert.ToString(dtCust.Rows[0]["CoTitle"]);
                custDetail.CoPName = Convert.ToString(dtCust.Rows[0]["CoPName"]);
                custDetail.CoMobileNo = Convert.ToString(dtCust.Rows[0]["CoMobileNo"]);
                custDetail.CoPAN = Convert.ToString(dtCust.Rows[0]["CoPAN"]);
                custDetail.CoAddress1 = Convert.ToString(dtCust.Rows[0]["CoAddress1"]);
                custDetail.CoAddress2 = Convert.ToString(dtCust.Rows[0]["CoAddress2"]);
                custDetail.CoCity = Convert.ToString(dtCust.Rows[0]["CoCity"]);
                custDetail.CoState = Convert.ToString(dtCust.Rows[0]["CoState"]);
                custDetail.CoCountry = Convert.ToString(dtCust.Rows[0]["CoCountry"]);
                custDetail.CoPinCode = Convert.ToString(dtCust.Rows[0]["CoPinCode"]);

                if (dtCust.Rows[0]["CoDOB"].ToString() == null || dtCust.Rows[0]["CoDOB"].ToString() == "")
                    custDetail.CoDOBst = "";
                else
                    custDetail.CoDOBst = Convert.ToDateTime(dtCust.Rows[0]["CoDOB"]).ToString("dd/MM/yyyy");
                custDetail.SecCoAppTitle = Convert.ToString(dtCust.Rows[0]["SecCoAppTitle"]);
                custDetail.SecCoPName = Convert.ToString(dtCust.Rows[0]["SecCoPName"]);
                custDetail.SecCoFName = Convert.ToString(dtCust.Rows[0]["SecCoFName"]);
                custDetail.SecCoMName = Convert.ToString(dtCust.Rows[0]["SecCoMName"]);
                custDetail.SecCoLName = Convert.ToString(dtCust.Rows[0]["SecCoLName"]);
                custDetail.SecCoAddress1 = Convert.ToString(dtCust.Rows[0]["SecCoAddress1"]);
                custDetail.SecCoAddress2 = Convert.ToString(dtCust.Rows[0]["SecCoAddress2"]);
                custDetail.SecCoMobileNo = Convert.ToString(dtCust.Rows[0]["SecCoMobileNo"]);
                custDetail.SecCoPAN = Convert.ToString(dtCust.Rows[0]["SecCoPAN"]);
                custDetail.SecCoCity = Convert.ToString(dtCust.Rows[0]["SecCoCity"]);
                custDetail.SecCoState = Convert.ToString(dtCust.Rows[0]["SecCoState"]);
                custDetail.SecCoCountry = Convert.ToString(dtCust.Rows[0]["SecCoCountry"]);
                custDetail.SecCoPinCode = Convert.ToString(dtCust.Rows[0]["SecCoPinCode"]);
                string sodob = dtCust.Rows[0]["SecCoDOB"].ToString();
                if (sodob == null || sodob == "")
                    custDetail.SecCoDOBst = "";
                else
                    custDetail.SecCoDOBst = Convert.ToDateTime(dtCust.Rows[0]["SecCoDOB"]).ToString("dd/MM/yyyy");
                custDetail.ExecutiveName = Convert.ToString(dtCust.Rows[0]["ExecutiveName"]);
                if (dtCust.Rows[0]["LoanAmount"].ToString() == null || dtCust.Rows[0]["LoanAmount"].ToString() == "")
                    custDetail.LoanAmount = 0;
                else
                    custDetail.LoanAmount = Convert.ToDecimal(dtCust.Rows[0]["LoanAmount"]);
                custDetail.BankBranch = Convert.ToString(dtCust.Rows[0]["BankBranch"]);
                if (dtCust.Rows[0]["BankID"].ToString() == null || dtCust.Rows[0]["BankID"].ToString() == "")
                    custDetail.BankID = 0;
                else
                    custDetail.BankID = Convert.ToInt32(dtCust.Rows[0]["BankID"]);
                custDetail.BankAddress = Convert.ToString(dtCust.Rows[0]["BankAddress"]);
                custDetail.Remarks = Convert.ToString(dtCust.Rows[0]["Remarks"]);
                custDetail.LienField = Convert.ToString(dtCust.Rows[0]["LienField"]);

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(custDetail);
        }

        public string updateCustomer(CustomerModel custDetail)
        {
            string st = custService.updateCustomer(custDetail);
           return st;
        }
        #endregion
    }
}