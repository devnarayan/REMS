using AutoMapper;
using REMS.Data;
using REMS.Data.Access.Master;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Master.Controllers
{
    public class AOChargeController : Controller
    {
        // GET: Master/AChargesCtrl
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddCharges()
        {
            return View();
        }
        #region AdditionalCharge
        AOChargeService adcservice = new AOChargeService();
        public string SaveAddOnCharge(AddOnchargeModel ads)
        {
            if (ads.Mandatory == null) ads.Mandatory = false;

            Mapper.CreateMap<AddOnchargeModel, AddOnCharge>();
            var model = Mapper.Map<AddOnchargeModel, AddOnCharge>(ads);
            int i = adcservice.AddAddOnCharge(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string EditAddOnCharge(AddOnchargeModel ads)
        {
            if (ads.Mandatory == null) ads.Mandatory = false;

            Mapper.CreateMap<AddOnchargeModel, AddOnCharge>();
            var model = Mapper.Map<AddOnchargeModel, AddOnCharge>(ads);
            int i = adcservice.EditAddOnCharge(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string DeleteAddOnCharge(int additionalChargeid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(adcservice.DeleteAddOnCharge(additionalChargeid));
        }
        public string GetAddOnCharge(int additionalChargeid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(adcservice.GetAddOnChargeByID(additionalChargeid));
        }
        public string AddOnChargeList()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(adcservice.GetAddOnChargeList());
        }
        #endregion
    }
}