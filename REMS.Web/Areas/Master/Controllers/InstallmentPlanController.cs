using REMS.Data.Access.Master;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Master.Controllers
{
    public class InstallmentPlanController : Controller
    {
        // GET: Master/InstallmentPlan
        public ActionResult Index()
        {
            return View();
        }

        #region PlanInstallment
        PlanInstallmentService iplansrvice = new PlanInstallmentService();
        public string SavePlanInstallment(PlanInstallmentModel planinstallment)
        {
            int i = iplansrvice.AddPlanInstallment(planinstallment);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string EditPlanInstallment(PlanInstallmentModel planinstallment)
        {
            int i = iplansrvice.EditPlanInstallment(planinstallment);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }

        public string GetPlanInstallment(int planInstallmentid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(iplansrvice.GetPlanInstallmentByID(planInstallmentid));
        }
        public string GetPlanInstallmentByPlanID(int planID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(iplansrvice.GetPlanInstallmentByPlanID(planID));
        }
        public string AllPlanList()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(iplansrvice.GetPlanInstallmentList());
        }
        #endregion
    }
}