using REMS.Data;
using REMS.Data.Access;
using REMS.Data.Access.Admin;
using REMS.Data.Access.Custo;
using REMS.Data.Access.Master;
using REMS.Data.Access.Sale;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class PropertyController : BaseController
    {
        DataFunctions obj = new DataFunctions();
        #region Private Fields
        PlanInstallmentService piService;
        PlanTypeMasterService ptService;
        FlatInstallmentService proService;
        SerivceTaxService staxService;
        SaleFlatService saleService;
        CustomerService custService;
        FlatService fltService;
        #endregion
        public PropertyController()
        {
            piService = new PlanInstallmentService();
            ptService = new PlanTypeMasterService();
            proService = new FlatInstallmentService();
            saleService = new SaleFlatService();
            staxService = new SerivceTaxService();
            custService = new CustomerService();
            fltService = new FlatService();
        }
        // GET: Sale/Property
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CalculatePrice(int Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }

        public ActionResult NewSale(int? Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult ViewInstallment(int? Id)
        {
            ViewBag.ID = Id;
            return View();
        }

        #region ChargeList
        public string GetFlatDetails(int flatid)
        {
            FlatService fservice = new FlatService();
            var model = fservice.GetFlatDetails(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatPLCList(int flatid)
        {
          var model=  fltService.GetFlatPLCList(flatid);
          return   Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatChargeList(int flatid)
        {
            var model = fltService.GetFlatChargeList(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatoChargeList(int flatid)
        {
            var model = fltService.GetFlatOChargeList(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        public JsonResult GetFlatsaleId(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();
            //int FId = Convert.ToInt32(flatid);
            //int sid = (from s in context.SaleFlats where s.FlatID == FId select s.SaleID).FirstOrDefault();
            //var md = (from s in context.SaleFlats join c in context.Customers on s.SaleID equals c.SaleID where c.SaleID == sid select new { Sale = s, CustomerName = (c.FName + " " + c.LName) }).FirstOrDefault();
            ////List<FlatSaleModel> model = new List<FlatSaleModel>();
            ////foreach (var v in md)
            ////{
            ////    string bdate = "";
            ////    if (v.Sale.SaleDate != null)
            ////        bdate = Convert.ToDateTime(v.Sale.SaleDate).ToString("dd/MM/yyyy");
            ////    model.Add(new FlatSaleModel { BookingDateSt = bdate, FlatID = v.Sale.FlatID, FName = v.CustomerName, BookingDate = v.Sale.SaleDate, BookingAmount = v.Sale.TotalAmount, Remarks = v.Sale.Remarks, SaleID = v.Sale.SaleID, FlatName = v.FlatName });
            ////}
            //return Newtonsoft.Json.JsonConvert.SerializeObject(md);
            //return Newtonsoft.Json.JsonConvert.SerializeObject(sid);
            List<SaleFlat> lstPropertyDetails = new List<SaleFlat>();
            int FId = Convert.ToInt32(flatid);
            int sid = (from s in context.SaleFlats where s.FlatID == FId select s.SaleID).FirstOrDefault();
            var payment = obj.GetDataTable("Select s.SaleID,s.FlatID,s.Aggrement,s.SaleDate, c.* from saleflat s inner join Customer c on s.SaleID=c.SaleID  where s.saleid='" + sid + "'");
            var v = payment.AsEnumerable().ToList();
            return Json(new { Sale = (from s in v select new { SaleID = s["SaleID"], SaleDate = s["SaleDate"], CustomerName = s["AppTitle"] + " " + s["FName"] + " " + s["MName"] + " " + s["LName"], PName = s["PName"], Mobile = s["MobileNo"], EmailID = s["EmailID"], CustomerID = s["CustomerID"], FlatID = s["FlatID"] }) }, JsonRequestBehavior.AllowGet);
        }
        //public string AddCustomer(CustomerModel custDetail)
        //{
        //    int custid = custService.AddCustomer(custDetail);
        //    return st;
        //}
        public string GetPlanTypeMasterByParams(string PlanName, string FType, decimal? Size)
        {
            PlanTypeMasterService ptmservice = new PlanTypeMasterService();
            var model = ptmservice.GetPlanTypeMasterByParams(PlanName, FType, Size);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        #endregion

        #region Installment Plan
        public string GetInstallmentPlan(string PlanName, string FType, decimal? Size, bool plc, bool acharge, bool aocharge, string bookdate)
        {
            var plantypemaster = ptService.GetPlanTypeMasterByParams(PlanName, FType, Size);
            int planid = (int)plantypemaster.PlanID;
            var model = piService.GetPlanInstallmentByPlanID(planid);
            string[] html = new string[3];
            string[] planhtml = planRowHtml(plc, acharge, aocharge);
            string rowhtml = "";
            int index = 0;
            DateTime bdate = Convert.ToDateTime(bookdate, DataHelper.IndianDateFormat());
            foreach (var md in model)
            {
                bdate = Convert.ToDateTime(bdate, DataHelper.IndianDateFormat()).AddDays((double)md.IntervalDays);
                string st = planhtml[0];
                st = st.Replace("<% SN %>", md.InstallmentNo.Value.ToString())
                    .Replace("<% InstallmentID %>", "INS" + md.PlanInstallmentID.ToString())
                    .Replace("<% BSPID %>", "BSP" + md.PlanInstallmentID.ToString())
                    .Replace("<% BSPValue %>", md.Amount.Value.ToString().TrimEnd('0').TrimEnd('.'))
                    .Replace("<% Options %>", installmentOptionhtml(model, index))
                    .Replace("<% PLCID %>", "PLC" + md.PlanInstallmentID.ToString())
                    .Replace("<% AdditionalChargeID %>", "ANC" + md.PlanInstallmentID.ToString())
                    .Replace("<% DueDate %>", "DueDate" + md.PlanInstallmentID.ToString())
                    .Replace("<% DueDateValue %>", bdate.ToString("dd/MM/yyyy"))
                    .Replace("<% RowNumber %>", index.ToString())
                    .Replace("<% AddOnChargeID %>", "AOC" + md.PlanInstallmentID.ToString());
                if (index == 0)
                {
                    st = st.Replace("<% PLCValue %>", "100")
                        .Replace("<% AdditionalChargeValue %>", "100")
                        .Replace("<% AddOnChargeValue %>", "100");
                }
                else
                {
                    st = st.Replace("<% PLCValue %>", "0")
                       .Replace("<% AdditionalChargeValue %>", "0")
                       .Replace("<% AddOnChargeValue %>", "0");
                }
                rowhtml += st;
                index = index + 1;
            }
            html[0] = rowhtml;
            html[1] = planhtml[1];
            html[2] = planhtml[2];
            return Newtonsoft.Json.JsonConvert.SerializeObject(html);
        }
        public string installmentOptionhtml(List<PlanInstallmentModel> model, int i)
        {
            string html = "";
            int index = 0;
            foreach (var md in model)
            {
                if (i == index)
                    html += "<option value='" + md.PlanInstallmentID.ToString() + "' selected>" + md.Installment + "</option>";
                else
                    html += "<option value='" + md.PlanInstallmentID.ToString() + "'>" + md.Installment + "</option>";
                index = index + 1;
            }
            return html;
        }

        public string[] planRowHtml(bool plc, bool additionalcharge, bool addoncharge)
        {
            Random rd = new Random();
            int rno = rd.Next(1, 7);
            string[] st = new string[3];
            string fhtml = @"<tr><td></td><td><b>Total</b></td><td><b id='TotalBSP'></b></td>";
            string hhtml = @"<tr><th>SN</th><th>Installment</th><th>BSP</th>";
            string html = @"<tr id='<% RowNumber %>'>";
            html += @"<td><% SN %></td>
                                        <td><label class='select'><select id='<% InstallmentID %>' name='ddlInstallment'>
                                        <% Options %>
                                        </select></label></td>";
            html += @"<td><label class='input'><input type='text' id='<% BSPID %>' name='txtBSP'  onblur='BSPChange()' value='<% BSPValue %>'></label></td>";

            if (plc)
            {
                hhtml += "<th>PLC</th>";
                fhtml += "<td><b id='TotalPLC'></b></td>";
                html += @"<td><label class='input'><input type='text' id='<% PLCID %>' name='txtPLC' onblur='PLCChange()' value='<% PLCValue %>'></label></td>";
            }
            if (additionalcharge)
            {
                hhtml += "<th>AdditionalCharge</th>";
                fhtml += "<td><b id='TotalACharge'></b></td>";
                html += @"<td><label class='input'><input type='text' id='<% AdditionalChargeID %>' name='txtACharge'  onblur='AChargeChange()'  value='<% AdditionalChargeValue %>'></label></td>";
            }
            if (addoncharge)
            {
                hhtml += "<th>AddOnCharge</th>";
                fhtml += "<td><b id='TotalAOCharge'></b></td>";
                html += @"<td><label class='input'><input type='text' id='<% AddOnChargeID %>'  name='txtAOCharge' onblur='AOChargeChange()'  value='<% AddOnChargeValue %>'></label></td>";
            }
            html += @" <td><label class='input'><i class='icon-append fa fa-calendar'></i><input type='text' id='<% DueDate %>' name='txtDueDate' value='<% DueDateValue %>' placeholder='Installment Due Date' class='datepicker' data-dateformat='dd/mm/yy'></label></td>";
            html += @" <td><i class='fa fa-trash-o'  onclick='DeletePlanRow(<% RowNumber %>)'></i></td></tr>";
            hhtml += @"<th>DueDate</th><th>Action</th></tr>";
            fhtml += @"<td></td><td></td></tr>";
            st[0] = html;
            st[1] = hhtml;
            st[2] = fhtml;
            return st;
        }

        public string AddInstallmentPlanRow(string PlanName, string FType, decimal? Size, bool plc, bool acharge, bool aocharge)
        {
            var plantypemaster = ptService.GetPlanTypeMasterByParams(PlanName, FType, Size);
            int planid = (int)plantypemaster.PlanID;
            var model = piService.GetPlanInstallmentByPlanID(planid);
            string[] planhtml = planRowHtml(plc, acharge, aocharge);
            string rowhtml = "";
            int index = 0;
            Random rd = new Random((int)DateTime.Now.Ticks);
            long ijk = (long)Math.Round(rd.NextDouble() * (99999999 - 10000000 - 1)) + 10000000;
            foreach (var md in model)
            {
                // add only one row
                if (index == 0)
                {
                    string st = planhtml[0];
                    st = st.Replace("<% SN %>", md.InstallmentNo.Value.ToString())
                        .Replace("<% InstallmentID %>", "INS" + md.PlanInstallmentID.ToString())
                        .Replace("<% BSPID %>", "BSP" + md.PlanInstallmentID.ToString())
                        .Replace("<% BSPValue %>", md.Amount.Value.ToString().TrimEnd('0').TrimEnd('.'))
                        .Replace("<% Options %>", installmentOptionhtml(model, index))
                        .Replace("<% PLCID %>", "PLC" + md.PlanInstallmentID.ToString())
                        .Replace("<% AdditionalChargeID %>", "ANC" + md.PlanInstallmentID.ToString())
                        .Replace("<% DueDate %>", "DueDate" + md.PlanInstallmentID.ToString())
                        .Replace("<% DueDateValue %>", "")
                        .Replace("<% RowNumber %>", ijk.ToString())
                        .Replace("<% AddOnChargeID %>", "AOC" + md.PlanInstallmentID.ToString());

                    st = st.Replace("<% PLCValue %>", "0")
                       .Replace("<% AdditionalChargeValue %>", "0")
                       .Replace("<% AddOnChargeValue %>", "0");
                    rowhtml += st;
                    index = index + 1;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(rowhtml);

        }

        public string SaveInstallment(int flatid, string Installment, string bsp, string plc, string acharges, string ocharges, string dueDate, decimal? InstallmentTotal, decimal? PLCTotal, decimal? AChargeTotal, decimal? OChargeTotal)
        {
            bool bl = false;
            string[] install = Installment.TrimStart(':').Split(':');
            string[] bp = bsp.TrimStart(':').Split(':');
            string[] pc = plc.TrimStart(':').Split(':');
            string[] acharg = acharges.TrimStart(':').Split(':');
            string[] ocharg = ocharges.TrimStart(':').Split(':');
            string[] dDate = dueDate.TrimStart(':').Split(':');
            for (int i = 0; i < install.Length; i++)
            {
                FlatInstallmentDetailModel mdl = new FlatInstallmentDetailModel();
                mdl.FlatID = flatid;
                mdl.PlanInstallmentID = Convert.ToInt32(install[i]);
                decimal? tamt = 0;
                if (bp[i] != null)
                {
                    mdl.BSPAmount = InstallmentTotal * Convert.ToDecimal(bp[i]) / 100;
                    mdl.BSPPer = Convert.ToDecimal(bp[i]);
                    tamt += InstallmentTotal * Convert.ToDecimal(bp[i]) / 100; ;
                }
                else
                {
                    mdl.BSPPer = 0;
                    mdl.BSPAmount = 0;
                }
                if (plc != "")
                {
                    mdl.PLCAmount = PLCTotal * Convert.ToDecimal(pc[i]) / 100;
                    mdl.PLCPer = Convert.ToDecimal(pc[i]);
                    tamt += mdl.PLCAmount;
                }
                else
                {
                    mdl.PLCPer = 0;
                    mdl.PLCAmount = 0;
                }
                if (acharges != "")
                {
                    mdl.AdditionalCAmount = AChargeTotal * Convert.ToDecimal(acharg[i]) / 100;
                    mdl.AdditionalPer = Convert.ToDecimal(acharg[i]);
                    tamt += mdl.AdditionalCAmount;
                }
                else
                {
                    mdl.AdditionalCAmount = 0;
                    mdl.AdditionalPer = 0;
                }
                if (ocharges != "")
                {
                    mdl.OptionalPer = Convert.ToDecimal(ocharg[i]);
                    mdl.OptionalCAmount = OChargeTotal * Convert.ToDecimal(ocharg[i]) / 100;
                    tamt += mdl.OptionalCAmount;
                }
                else
                {
                    mdl.OptionalPer = 0;
                    mdl.OptionalCAmount = 0;
                }
                if (dDate[i] != null && dDate[i] != "" && dDate[i] != "NaN")
                    mdl.DueDate = Convert.ToDateTime(dDate[i], DataHelper.IndianDateFormat());
                mdl.TotalAmount = tamt;
                mdl.TotalAmtInWords = "";
                mdl.RecordStatus = 1;
                mdl.InstallmentOrder = i + 1;
                mdl.Activity = "Y";
                mdl.CreateDate = DateTime.Now;
                mdl.UserName = User.Identity.Name;
                mdl.InstallmentServiceTaxID = staxService.GetServiceTax().ServiceTaxID;
                int ii = proService.AddFlatInstallment(mdl);
                if (ii > 0)
                    bl = true;
            }
            return bl.ToString();
        }

        [WebMethod]
        public string SaveInstallment2(NewSaleModel sale)
        {
            try
            {
                bool bl = false;
                string[] install = sale.Installment.TrimStart(':').Split(':');
                string[] bp = sale.bsp.TrimStart(':').Split(':');
                string[] dDate = sale.dueDate.TrimStart(':').Split(':');
                for (int i = 0; i < install.Length; i++)
                {
                    FlatInstallmentDetailModel mdl = new FlatInstallmentDetailModel();
                    mdl.FlatID = sale.FlatsID;
                    mdl.PlanInstallmentID = Convert.ToInt32(install[i]);
                    decimal? tamt = 0;
                    if (bp[i] != null)
                    {
                        mdl.BSPAmount = sale.InstallmentTotal * Convert.ToDecimal(bp[i]) / 100;
                        mdl.BSPPer = Convert.ToDecimal(bp[i]);
                        tamt += sale.InstallmentTotal * Convert.ToDecimal(bp[i]) / 100;
                    }
                    else
                    {
                        mdl.BSPPer = 0;
                        mdl.BSPAmount = 0;
                    }
                    if (sale.plc != null)
                    {
                        string[] pc = sale.plc.TrimStart(':').Split(':');
                        mdl.PLCAmount = sale.PLCTotal * Convert.ToDecimal(pc[i]) / 100;
                        mdl.PLCPer = Convert.ToDecimal(pc[i]);
                        tamt += mdl.PLCAmount;
                    }
                    else
                    {
                        mdl.PLCPer = 0;
                        mdl.PLCAmount = 0;
                    }
                    if (sale.acharges != null)
                    {
                        string[] acharg = sale.acharges.TrimStart(':').Split(':');
                        mdl.AdditionalCAmount = sale.AChargeTotal * Convert.ToDecimal(acharg[i]) / 100;
                        mdl.AdditionalPer = Convert.ToDecimal(acharg[i]);
                        tamt += mdl.AdditionalCAmount;
                    }
                    else
                    {
                        mdl.AdditionalCAmount = 0;
                        mdl.AdditionalPer = 0;
                    }
                    if (sale.ocharges != null)
                    {
                        string[] ocharg = sale.ocharges.TrimStart(':').Split(':');
                        mdl.OptionalPer = Convert.ToDecimal(ocharg[i]);
                        mdl.OptionalCAmount = sale.OChargeTotal * Convert.ToDecimal(ocharg[i]) / 100;
                        tamt += mdl.OptionalCAmount;
                    }
                    else
                    {
                        mdl.OptionalPer = 0;
                        mdl.OptionalCAmount = 0;
                    }
                    if (dDate[i] != null && dDate[i] != "" && dDate[i] != "NaN")
                        mdl.DueDate = Convert.ToDateTime(dDate[i], DataHelper.IndianDateFormat());
                    mdl.TotalAmount = tamt;
                    mdl.TotalAmtInWords = "";
                    mdl.RecordStatus = 1;
                    mdl.InstallmentOrder = i + 1;
                    mdl.Activity = "Y";
                    mdl.CreateDate = DateTime.Now;
                    mdl.UserName = User.Identity.Name;
                    mdl.InstallmentServiceTaxID = staxService.GetServiceTax().ServiceTaxID;
                    mdl.InsVersion = 0;
                    int ii = proService.AddFlatInstallment(mdl);
                    if (ii > 0)
                        bl = true;
                }
                return bl.ToString();
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return "false";
            }
        }


        public string GetFlatInstallments(int flatid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(proService.GetFlatInstallment(flatid));
        }
        public string GetFlatInstallmentWithCharges(int flatid, decimal flatsize, int version)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(proService.GetFlatInstallmentWithCharges(flatid, flatsize, version));
        }
        public string GetFlatInstallmentWithCharges2(int FlatID, decimal flatsize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(proService.GetFlatInstallmentWithCharges(FlatID, flatsize, 0));
        }
        public string DeleteInstallment(int flatid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(proService.DeleteFlatInstallment(flatid));
        }

        public string SaleFlatSave(int flatid, decimal? salerate, string saleDate, string saletype, string planName)
        {
            if (!saleService.IsSaled(flatid))
            {

                SaleFlatModel model = new SaleFlatModel();
                model.FlatID = flatid;
                model.TotalAmount = salerate;
                model.SaleDate = Convert.ToDateTime(saleDate, DataHelper.IndianDateFormat());
                model.Status = saletype;
                model.DemandStatus = 0;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = User.Identity.Name.ToString();
                model.PlanName = planName;
                ProjectService projService = new ProjectService();
                model.ProjectID = projService.GetProjectByFlatID(flatid).ProjectID;
                int i = saleService.AddSaleFlat(model);
                if (i > 0)
                {
                    // Insert into customer
                    CustomerModel cust = new CustomerModel();
                    cust.SaleID = i;
                    cust.SaleStatus = true;
                    //cust.SaleStatus = saletype; 
                    int ii = custService.AddCustomer(cust);
                    return ii.ToString();
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                // Update SaleFlat
                var model = new SaleFlatModel
                {
                    FlatID = flatid,
                    UpdateBy = User.Identity.Name.ToString(),
                    UpdateDate = DateTime.Now,
                    Status = saletype,
                    PlanName = planName,
                    TotalAmount = salerate,
                    SaleDate = Convert.ToDateTime(saleDate, DataHelper.IndianDateFormat()),
                };
                int i = saleService.EditSaleFlatRegenerate(model);
                return i.ToString();
            }
        }

        public string SaleFlatTransfer(string AppTitle, string FName, string MName, string LName, int salecustid, int OldCustId, string TransferAmount, string TDate)
        {
            System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            Hashtable htPayment = new Hashtable();
            htPayment.Add("CustomerID", OldCustId);
            htPayment.Add("AppTitle", AppTitle);
            htPayment.Add("FName", FName);
            htPayment.Add("MName", MName);
            htPayment.Add("LName", LName);
            htPayment.Add("SaleID", salecustid);
            htPayment.Add("TransferAmount", TransferAmount);
            htPayment.Add("UserName", User.Identity.Name);
            htPayment.Add("TransferDate", Convert.ToDateTime(TDate, dtinfo));
            // htPayment.Add("IsBounce", IsBounce);
            if (obj.ExecuteProcedure("Update_TransferProperty", htPayment))
            {
                string st = "Yes";
                return Newtonsoft.Json.JsonConvert.SerializeObject(st);
            }
            string stt = "No";
            return Newtonsoft.Json.JsonConvert.SerializeObject(stt);

        }


        #endregion

        #region Edit Flat Charge
        public string EditFlatCharge(int flatID, int editTypeID, string editType, string amount)
        {
            Hashtable ht = new Hashtable();
            ht.Add("FlatID", flatID);
            ht.Add("EditType", editType);
            ht.Add("EditTypeID", editTypeID);
            ht.Add("Amount", amount);
            bool bl = obj.ExecuteProcedure("spEditFlatAmount", ht);
            // Update FlatSale Status
            fltService.UpdateFlatSaleStatus(flatID, User.Identity.Name.ToString(), "Regenerate");
            return Newtonsoft.Json.JsonConvert.SerializeObject(bl);
        }
        public string DeleteFlatAttribute(int flatID, int editTypeID, string editType, string amount)
        {
            Hashtable ht = new Hashtable();
            ht.Add("FlatID", flatID);
            ht.Add("EditType", editType);
            ht.Add("EditTypeID", editTypeID);
            bool bl = obj.ExecuteProcedure("spDeleteFlatAmount", ht);
            // Update FlatSale Status
            fltService.UpdateFlatSaleStatus(flatID, User.Identity.Name.ToString(), "Regenerate");
            return Newtonsoft.Json.JsonConvert.SerializeObject(bl);
        }
        #endregion

        #region View Installments
        public string GetInstallmentVersion(int flatid)
        {
            var model = proService.GetInstallmentVersion(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        #endregion
    }
}