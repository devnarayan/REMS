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
    public class AChargesCtrlController : Controller
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
        AdditionalChargeService adcservice = new AdditionalChargeService();
        public string SaveAdditionalCharge(AdditionalChargeModel ads)
        {
            if (ads.Mandatory == null) ads.Mandatory = false;

            Mapper.CreateMap<AdditionalChargeModel, AdditionalCharge>();
            var model = Mapper.Map<AdditionalChargeModel, AdditionalCharge>(ads);
            int i = adcservice.AddAdditionalCharge(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string EditAdditionalCharge(AdditionalChargeModel ads)
        {
            if (ads.Mandatory == null) ads.Mandatory = false;

            Mapper.CreateMap<AdditionalChargeModel, AdditionalCharge>();
            var model = Mapper.Map<AdditionalChargeModel, AdditionalCharge>(ads);
            int i = adcservice.EditAdditionalCharge(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string DeleteAdditionalCharge(int additionalChargeid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(adcservice.DeleteAdditionalCharge(additionalChargeid));
        }

        public string GetAdditionalCharge(int additionalChargeid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(adcservice.GetAdditionalChargeByID(additionalChargeid));
        }
        public string AditionalChargeList()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(adcservice.GetAdditionalChargeList());
        }
        #endregion
    }
}