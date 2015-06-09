using REMS.Data;
using REMS.Data.Access;
using REMS.Data.CustomModel;
using REMS.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Customer.Controllers
{

    public class RemarkController : Controller
    {
        //
        // GET: /Customer/Remark/
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult PropertyRemark(int? id)
        {
            ViewBag.SaleID = id;
            return View();
        }

        public ActionResult AddRemark(int? id)
        {
            ViewBag.SaleID = id;
            return View();
        }

        public string SearchPropertyRemak(string search)
        {
            try
            {
                string squrty = "select * from dbo.PropertyRemark where Rid in( (select  MAX(Rid)  from dbo.PropertyRemark group by SaleID))and  ProprtyName ='" + search + "' order by Rid desc";

                REMSDBEntities context = new REMSDBEntities();
                DataFunctions obj = new DataFunctions();
                List<FlatSaleModel> model = new List<FlatSaleModel>();
                DataTable dt = obj.GetDataTable(squrty);
                var status = "InActive";
                foreach (DataRow bankDetails in dt.Rows)
                {
                    var RemakrDate = Convert.ToDateTime(bankDetails["RemakDate"]).ToString("dd/MM/yyyy");
                    if (bankDetails["status"].ToString() == "1")
                    {
                        status = "Active";
                    }
                    else
                    {
                        status = "InActive";
                    }
                    model.Add(new FlatSaleModel { SaleID = Convert.ToInt32(bankDetails["SaleID"]), Rid = Convert.ToString(bankDetails["Rid"]), BookingDateSt = RemakrDate, FlatName = Convert.ToString(bankDetails["ProprtyName"]), Remarks = Convert.ToString(bankDetails["Remark"]), StRStatus = status, Dueamount = Convert.ToDecimal(bankDetails["FollowupAmount"]), CreateBy = Convert.ToString(bankDetails["CreatedBy"]) });

                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public string SavePropertyRemak(string propertyid, string amt, string propertyName, string datefrom, string Remark, string saleid)
        {
            try
            {
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                dtinfo.DateSeparator = "/";
                int SaleID = 0;
                if (saleid != "")
                {
                    SaleID = Convert.ToInt32(saleid);
                }
                REMSDBEntities context = new REMSDBEntities();
                PropertyRemark _PropertyRemark = new PropertyRemark();
                _PropertyRemark.RemakDate = Convert.ToDateTime(datefrom, dtinfo);
                _PropertyRemark.Remark = Remark;
                _PropertyRemark.SaleID = SaleID;
                _PropertyRemark.status = 1;
                _PropertyRemark.FollowupAmount = Convert.ToDecimal(amt);
                _PropertyRemark.createdate = DateTime.Now;
                _PropertyRemark.ProprtyName = propertyName;
                _PropertyRemark.CreatedBy = User.Identity.Name;
                context.PropertyRemarks.Add(_PropertyRemark);

                context.SaveChanges();


                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }
        public string GetAllRemark(string saleid)
        {
            try
            {
                int SaleID = 0;
                if (saleid != "")
                {
                    SaleID = Convert.ToInt32(saleid);
                }

                REMSDBEntities context = new REMSDBEntities();
                List<FollowupModel> model = new List<FollowupModel>();
                var VALUE1 = (from PR in context.PropertyRemarks
                              where PR.SaleID == SaleID
                              select new { ID = PR.Rid, Remark = PR.Remark, Remakrdate = PR.RemakDate, SaleID = PR.SaleID, ProprtyName = PR.ProprtyName, FAmount = PR.FollowupAmount, CreatedBy = PR.CreatedBy }).ToList();
                foreach (var PRvalue in VALUE1)
                {
                    var RemakrDate = Convert.ToDateTime(PRvalue.Remakrdate).ToString("dd/MM/yyyy");
                    model.Add(new FollowupModel { Rid = PRvalue.ID, SaleID = SaleID, RemarkDateSt = RemakrDate, FlatName = PRvalue.ProprtyName, Remark = PRvalue.Remark, FollowupAmount = PRvalue.FAmount, CreatedBy = PRvalue.CreatedBy });
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {

                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public string DeleteRemark(string rid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    int? id = Convert.ToInt32(rid);
                    var md = context.PropertyRemarks.Where(r => r.Rid == id).FirstOrDefault();
                    context.Entry(md).State = EntityState.Deleted;
                    int i = context.SaveChanges();
                    return i.ToString();
                }
                catch (Exception ex)
                {
                    Helper h = new Helper();
                    h.LogException(ex);
                    return "0";
                }
            }
        }

    }
}