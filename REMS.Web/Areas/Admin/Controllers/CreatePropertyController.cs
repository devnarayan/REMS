using REMS.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using REMS.Data.Access.Admin;
using REMS.Data;
using AutoMapper;
using REMS.Web.App_Helpers;
using REMS.Data.DataModel;
using REMS.Data.CustomModel;

namespace REMS.Web.Areas.Admin.Controllers
{
    public class CreatePropertyController : Controller
    {
        // Angular Service
        TowerService tservice = new TowerService();
        FlatService fservice = new FlatService();
        FloorService flrService = new FloorService();
        // GET: Admin/CreateProperty
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddTower()
        {
            return View();
        }
        public ActionResult AddFloor()
        {
            return View();
        }
        public ActionResult AddFlat()
        {
            return View();
        }
        public ActionResult EditFlat(int? id)
        {
            ViewBag.FlatID = id;
            // get floor id of flat id(id).
            if (id != 0) {
                var fmodel= fservice.GetFlatByID((int)id);
                ViewBag.FloorID = fmodel.FloorID;
            }
            return View();
        }
        public ActionResult UpdateFlaType()
        {
            return View();
        }
       
      
        #region TowerService
        public string SaveTower(TowerModel tower)
        {
            if (tservice.IsTower(tower.TowerName))
            {
                return (2).ToString();
            }
            else
            {
                TowerService ts = new TowerService();
                Mapper.CreateMap<TowerModel, Tower>();
                Tower tw = Mapper.Map<TowerModel, Tower>(tower);
                tw.CrBy = User.Identity.Name;
                tw.CrDate = DateTime.Now;
                tw.TowerNo = tw.TowerName;
                int i = ts.AddTower(tw);
                return i.ToString();
            }
        }
        public string UpdateTower(TowerModel tower)
        {
            TowerService ts = new TowerService();
            Mapper.CreateMap<TowerModel, Tower>();
            Tower tw = Mapper.Map<TowerModel, Tower>(tower);
            tw.CrBy = User.Identity.Name;
            tw.CrDate = DateTime.Now;
            tw.TowerNo = tw.TowerName;
            int i = ts.EditTower(tw);
            return i.ToString();
        }
        public string getAllTower()
       {
            List<Tower> ts = new List<Tower>();
            List<TowerModel> tm = new List<TowerModel>();
            ts = tservice.AllTower();
            foreach (var md in ts)
            {
                Mapper.CreateMap<Tower, TowerModel>();
                var tmdl = Mapper.Map<Tower, TowerModel>(md);
                if (tmdl.PossessionDate != null)
                    tmdl.PossessionDateSt = tmdl.PossessionDate.Value.ToString("dd/MM/yyyy");
                tm.Add(tmdl);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(tm);
        }
        public string getTowerbyID(int towerid)
        {
            Tower tw = tservice.TowerList(towerid);
            Mapper.CreateMap<Tower, TowerModel>();
            var model = Mapper.Map<Tower, TowerModel>(tw);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string AllTowerProject()
        {
            var model = tservice.AllTowerProject();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        #endregion
        #region FloorService
        public string SaveFloor(FloorModel floor)
        {
            Mapper.CreateMap<FloorModel, Floor>();
            Floor fl = Mapper.Map<FloorModel, Floor>(floor);
            fl.CrBy = User.Identity.Name;
            fl.CrDate = DateTime.Now;
            bool bl = flrService.IsFloorNo(floor.FloorNo, floor.TowerID);
            if (bl == true) { return (2).ToString(); }
            else
            {
                int i = flrService.AddFloor(fl);
                return i.ToString();

            }
        }
        public string UpdateFloor(FloorModel floor)
        {
            Mapper.CreateMap<FloorModel, Floor>();
            Floor fl = Mapper.Map<FloorModel, Floor>(floor);
            int i = flrService.EditFloor(fl);
            return i.ToString();
        }
        public string getFloorByID(int floorid)
        {
            Floor fl = flrService.FloorList(floorid);
            Mapper.CreateMap<Floor, FloorModel>();
            var model = Mapper.Map<Floor, FloorModel>(fl);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string getAllFloorByTowerID(int towerid)
        {
            List<FloorModel> fl = flrService.AllFloor(towerid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(fl);
        }
        public string DeleteFloor(int floorid)
        {
            int i = flrService.DeleteFloor(floorid);
            return i.ToString();
        }
        #endregion
        #region FlatService Edit

        public string SaveFat(FlatModel flat)
        {
            Mapper.CreateMap<FlatModel, Flat>().ForMember(dsd => dsd.FlatPLCs, df => df.Ignore());
            Flat fl = Mapper.Map<FlatModel, Flat>(flat);
            fl.CrBy = User.Identity.Name;
            fl.CrDate = DateTime.Now;
            int i = fservice.AddNewFlat(fl);
            FlatPLCService fpservice = new FlatPLCService();
            foreach (string st in flat.FlatPLCs)
            {
                if (st != "0")
                {
                    FlatPLC fpm = new FlatPLC();
                    fpm.FlatID = fl.FlatID;
                    fpm.PLCID = Convert.ToInt32(st);
                    int ii = fpservice.AddFlatPLC(fpm);
                }
            }
            return i.ToString();
        }
        public string UpdateFlat(FlatModel flat)
        {
            Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, sec => sec.Ignore());
            Flat fl = Mapper.Map<FlatModel, Flat>(flat);
            int i = fservice.EditFlat(fl,User.Identity.Name);
            FlatPLCService fpservice = new FlatPLCService();
            var model = fpservice.GetFlatPLCListByFlatID(fl.FlatID);

            foreach (var ft in model)
            {
                FlatPLC md = new FlatPLC();
                md.FlatPLCID = ft.FlatPLCID;
                int ii = fpservice.DeleteFlatPLC(md);
            }
            foreach (string st in flat.FlatPLCs)
            {
                if (st != "0")
                {
                    FlatPLC fpm = new FlatPLC();
                    fpm.FlatID = fl.FlatID;
                    fpm.PLCID = Convert.ToInt32(st);
                    int ii = fpservice.AddFlatPLC(fpm);
                }
            }
            // Update FlatSale Status
            fservice.UpdateFlatSaleStatus(Convert.ToInt32(flat.FlatID), User.Identity.Name.ToString(), "Regenerate");
            return i.ToString();
        }
        public string UpdateFlatStatus(int flatid, string status)
        {
          int i=  fservice.EditFlatStatus(flatid, status);
          return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string getFlatByID(int flatid)
        {
            FlatModel fl = fservice.GetFlatByID(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(fl);
        }
        public string GetFlatByTowerID(int towerid)
        {
          var model=  fservice.GetFlatListByTowerID(towerid);
          return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetReservedFlatListByTowerID(int towerid)
        {
            var model = fservice.GetReservedFlatListByTowerID(towerid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatByFlatNo(int towerid,int flatno)
        {
            var model = fservice.GetFlatListByTowerID(towerid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatsByFloorID(int floorid)
        {
            List<FlatModel> fl = fservice.GetFlatsByFloorID(floorid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(fl);
        }
        public string DeleteFlat(int flatid)
        {
            int i = fservice.DeleteFlat(flatid);
            return i.ToString();
        }
        public string GetPCLHtml()
        {
            string html = "";
            PLCService ps = new PLCService();
            var model = ps.GetAllPLC();
            html += "<div id='panelPLC'>";
            foreach (var pm in model)
            {
                string id = pm.PLCID.ToString();
                html += @"<label class='checkbox'><input type='checkbox' value='" + pm.PLCID + "' name='" + pm.PLCName + "' id='" + id + "'><i></i> " + pm.PLCName + "</label>";
            }
            html += "</div></div>";
            return html;
        }
        public string GetFlatPCLHtml(int flatid)
        {
            string html = "";
            PLCService ps = new PLCService();
            FlatPLCService fservice = new FlatPLCService();
            var model = ps.GetAllPLC();
            html += "<div id='panelPLC'>";
            foreach (var pm in model)
            {
                bool bl = fservice.IsPLCinFlat(pm.PLCID, flatid);
                if (bl == true)
                {
                    string id = pm.PLCID.ToString();
                    html += @"<label class='checkbox'><input type='checkbox' value='" + pm.PLCID + "' name='" + pm.PLCName + "' id='" + id + "' checked='checked'><i></i> " + pm.PLCName + "</label>";
                }
                else
                {
                    string id = pm.PLCID.ToString();
                    html += @"<label class='checkbox'><input type='checkbox' value='" + pm.PLCID + "' name='" + pm.PLCName + "' id='" + id + "'><i></i> " + pm.PLCName + "</label>";
                }
            }
            html += "</div></div>";
            return html;
        }
        #endregion
        #region GenerateFlat Property
        public string FlatHmtl(string TotalFlat)
        {
            string FltHtml = "";
            int flatcount = Convert.ToInt32(TotalFlat);
            for (int i = 1; i <= flatcount; i++)
            {
                string html = fservice.FlatCreateBody();
                PLCService ps = new PLCService();
                var model = ps.GetMandatoryPLC();
                html += "<div id='panel" + i.ToString() + "'>";
                foreach (var pm in model)
                {
                    string id = pm.PLCID.ToString() + " " + i.ToString();
                    html += @"<label class='checkbox'><input type='checkbox' value='" + pm.PLCID + "' name='" + pm.PLCName + "' id='" + id + "'><i></i> " + pm.PLCName + "</label>";
                }
                html += "</div></div>";
                FltHtml += html.Replace("<% FlatNo %>", "FlatNo" + i).Replace("<% PreIncrement %>", "PreIncrement" + i);
            }
            return FltHtml;
        }
        public string GenerateTowerAddFloor(int TotalFloor, int TowerID)
        {
            int i = fservice.AddFloor(TowerID, TotalFloor, User.Identity.Name);
            return i.ToString();
        }
        public string GenerateTowerAddFlat(int TotalFloor, List<int> FlatNo, List<bool> PreIncrement, string[] PLCIDs, int TowerID,string FltType, string FltTSize)
        {
            int i = 0;
            //foreach (int flat in FlatNo)
            //{
            List<PLCModel> pmodel = new List<PLCModel>();
            PLCModel pm = new PLCModel();
            foreach (string s in PLCIDs)
            {
                if (s != null && s != "")
                {
                    pmodel.Add(new PLCModel { PLCID = Convert.ToInt32(s), PLCName = s });
                }
            }
            int ss = fservice.AddFlats(TotalFloor, 0, FlatNo, PreIncrement, pmodel, User.Identity.Name, TowerID,FltType,FltTSize);
            i++;
            // }

            return "1";
        }
        #endregion

        #region FlatCharge
        FlatChargeService fcservice = new FlatChargeService();
        public string SaveFlatChage(FlatChargeModel model)
        {
            Mapper.CreateMap<FlatChargeModel, FlatCharge>();
            var mdl = Mapper.Map<FlatChargeModel, FlatCharge>(model);
            mdl.CrDate = DateTime.Now;
            int i = fcservice.AddFlatCharge(mdl);
            
            // Update FlatSale Status
            fservice.UpdateFlatSaleStatus(Convert.ToInt32(model.FlatID), User.Identity.Name.ToString(), "Regenerate");
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string UpdateFlatChage(FlatChargeModel model)
        {
            Mapper.CreateMap<FlatChargeModel, FlatCharge>();
            var mdl = Mapper.Map<FlatChargeModel, FlatCharge>(model);
            int i = fcservice.EditFlatCharge(mdl);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string DeleteFlatCharge(int FlatChargeID)
        {
            FlatCharge mdl = new FlatCharge();
            mdl.FlatChargeID = FlatChargeID;
            int i = fcservice.EditFlatCharge(mdl);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetFlatChage(int flatchargeid)
        {
            FlatChargeModel model = fcservice.GetFlatChargeByID(flatchargeid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatChageList(int flatid)
        {
            List<FlatChargeModel> model = fcservice.GetFlatChargeListByFlatID(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
      
        #endregion

        #region Flat AddOn Charge
        FlatOChargeService focservice = new FlatOChargeService();
        public string SaveFlatOChage(FlatOChargeModel model)
        {
            Mapper.CreateMap<FlatOChargeModel, FlatOCharge>();
            var mdl = Mapper.Map<FlatOChargeModel, FlatOCharge>(model);
            mdl.CrDate = DateTime.Now;
            int i = focservice.AddFlatOCharge(mdl);
            
            // Update FlatSale Status
            fservice.UpdateFlatSaleStatus(Convert.ToInt32(model.FlatID), User.Identity.Name.ToString(), "Regenerate");
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string UpdateFlatOChage(FlatOChargeModel model)
        {
            Mapper.CreateMap<FlatOChargeModel, FlatOCharge>();
            var mdl = Mapper.Map<FlatOChargeModel, FlatOCharge>(model);
            int i = focservice.EditFlatOCharge(mdl);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string DeleteFlatOCharge(int FlatOChargeID)
        {
            FlatOCharge mdl = new FlatOCharge();
            mdl.FlatOChargeID = FlatOChargeID;
            int i = focservice.EditFlatOCharge(mdl);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetFlatOChage(int flatochargeid)
        {
            FlatOChargeModel model = focservice.GetFlatOChargeByID(flatochargeid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatOChargeList(int flatid)
        {
            List<FlatOChargeModel> model = focservice.GetFlatOChargeListByFlatID(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        #endregion

        #region Update All FlatType
        public string UpdateAllFlatType(UpdateFlatTypeModel model)
        {
            FlatService fs = new FlatService();
            if (model.TowerID == 0)
            {
                if (model.FloorID == 0)
                {
                    return "0";
                }
                else
                {
                    fs.UpdateFlatTypeAllTowerPerFloor(model,User.Identity.Name);
                }
            }
            else
            {
                if (model.FloorID == 0)
                {
                    fs.UpdateFlatTypePerTowerAllFloor(model, User.Identity.Name);
                }
                else
                {
                    fs.UpdateFlatTypePerTowerPerFloor(model, User.Identity.Name);
                }
            }
            return "1";
        }
        #endregion
    }
}