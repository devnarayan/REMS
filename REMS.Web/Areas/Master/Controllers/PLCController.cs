using AutoMapper;
using REMS.Data;
using REMS.Data.Access.Admin;
using REMS.Data.Access.Master;
using REMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Master.Controllers
{
    public class PLCController : Controller
    {
        private SerivceTaxService staxService;
        public PLCController()
        {
            staxService = new SerivceTaxService();
        }
        // GET: Master/PLC
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPLC()
        {
            return View();
        }
        public ActionResult ManageServiceTax()
        {
            return View();
        }
        #region PLCMaster
        PLCService pservice = new PLCService();
        public string SavePLC(PLCModel plc)
        {
            if (plc.Mandatory == null) plc.Mandatory = false;
            int i = pservice.AddPLC(plc);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string EditPLC(PLCModel plc)
        {
            if (plc.Mandatory == null) plc.Mandatory = false;
            int i = pservice.EditPLC(plc);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }

        public string GetPLC(int plcid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(pservice.GetPLCbyID(plcid));
        }
        public string PLCList()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(pservice.GetAllPLC());
        }
        #endregion

        #region ServiceTaxMaster

        public string GetServiceTaxList()
        {
            var model = staxService.GetServiceTaxList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string AddServiceTax(ServiceTaxModel tax)
        {
            if (tax.Status == null) tax.Status = false;
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            if (tax.EndDateSt != null) tax.EndDate = Convert.ToDateTime(tax.EndDateSt, dtinfo);
            Mapper.CreateMap<ServiceTaxModel, ServiceTax>();
            var model = Mapper.Map<ServiceTaxModel, ServiceTax>(tax);
            int i = staxService.AddSerivceTaxService(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string UpdateServiceTax(ServiceTaxModel tax)
        {
            if (tax.Status == null) tax.Status = false;
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            if (tax.EndDateSt != null) tax.EndDate = Convert.ToDateTime(tax.EndDateSt, dtinfo);
            Mapper.CreateMap<ServiceTaxModel, ServiceTax>();
            var model = Mapper.Map<ServiceTaxModel, ServiceTax>(tax);
            int i = staxService.EditSerivceTaxService(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string SelectServiceTax(int ServiceTaxID)
        {
            var model = staxService.GetServiceTaxByID(ServiceTaxID);
            Mapper.CreateMap<ServiceTax, ServiceTaxModel>().ForMember(des=>des.EndDateSt,op=>op.MapFrom(os=>os.EndDate.Value.ToString("dd/MM/yyyy")));
            var mdl = Mapper.Map<ServiceTax, ServiceTaxModel>(model);
            return Newtonsoft.Json.JsonConvert.SerializeObject(mdl);
        }
        public string DeleteServiceTax(int ServiceTaxID)
        {
            int i= staxService.DeleteServiceTaxByID(ServiceTaxID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetServiceTaxByID(int ServiceTaxID)
        {
            var model = staxService.GetServiceTaxByID(ServiceTaxID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        #endregion
    }
}