using AutoMapper;
using REMS.Data;
using REMS.Data.Access.Admin;
using REMS.Data.Access.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Master.Controllers
{
    public class PLCController : Controller
    {
        // GET: Master/PLC
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPLC()
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
    }
}