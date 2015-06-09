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
    public class PlanCtrlController : Controller
    {
        // GET: Master/PlanCtrl
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPlan()
        {
            return View();
        }
        public ActionResult AddPlanInstallment()
        {
            return View();
        }
        public ActionResult PlanTypeSalePrice()
        {
            return View();
        }

        #region PlanTypeMaster Service
        public string SavePlanTypeMaster(PlanTypeMasterModel plan)
        {
            FlatTypeService ftservice = new FlatTypeService();
            FlatTypeSizeService ftsservice = new FlatTypeSizeService();
            var FlatTypemodel = ftservice.GetFlatTypeList();
            if (plan.FType == "0" && plan.FlatTypeSizeID == 0)
            {
                foreach (var flist in FlatTypemodel)
                {
                    var FSizemodel = ftsservice.GetFlatTypeSizeListByFlattypeid(flist.FlatTypeID);
                    foreach (var fsize in FSizemodel)
                    {
                        plan.FlatTypeSizeID = fsize.FlatTypeSizeID;
                        plan.FType = flist.FType;
                        PlanTypeMasterService ptmService = new PlanTypeMasterService();
                        Mapper.CreateMap<PlanTypeMasterModel, PlanTypeMaster>();
                        var model = Mapper.Map<PlanTypeMasterModel, PlanTypeMaster>(plan);
                        int i = ptmService.AddPlanTypeMaster(model);
                    }
                }
                return (1).ToString();
            }
            else if (plan.FType == "0" && plan.FlatTypeSizeID != 0)
            {
                foreach (var flist in FlatTypemodel)
                {
                    plan.FType = flist.FType;
                    PlanTypeMasterService ptmService = new PlanTypeMasterService();
                    Mapper.CreateMap<PlanTypeMasterModel, PlanTypeMaster>();
                    var model = Mapper.Map<PlanTypeMasterModel, PlanTypeMaster>(plan);
                    int i = ptmService.AddPlanTypeMaster(model);
                }
                return (1).ToString();
            }
            else if (plan.FType != "0" && plan.FlatTypeSizeID == 0)
            {
                var ftmodl=  ftservice.GetFlatTypeByFType(plan.FType);
                var FSizemodel = ftsservice.GetFlatTypeSizeListByFlattypeid(ftmodl.FlatTypeID);
                foreach (var fsize in FSizemodel)
                {
                    plan.FlatTypeSizeID = fsize.FlatTypeSizeID;
                    PlanTypeMasterService ptmService = new PlanTypeMasterService();
                    Mapper.CreateMap<PlanTypeMasterModel, PlanTypeMaster>();
                    var model = Mapper.Map<PlanTypeMasterModel, PlanTypeMaster>(plan);
                    int i = ptmService.AddPlanTypeMaster(model);
                }
                return (1).ToString();
            }
            else
            {
                PlanTypeMasterService ptmService = new PlanTypeMasterService();
                Mapper.CreateMap<PlanTypeMasterModel, PlanTypeMaster>();
                var model = Mapper.Map<PlanTypeMasterModel, PlanTypeMaster>(plan);
                int i = ptmService.AddPlanTypeMaster(model);
                return i.ToString();
            }
        }
        public string EditPlanTypeMaster(PlanTypeMasterModel plan)
        {
            PlanTypeMasterService ptmService = new PlanTypeMasterService();
            Mapper.CreateMap<PlanTypeMasterModel, PlanTypeMaster>();
            var model = Mapper.Map<PlanTypeMasterModel, PlanTypeMaster>(plan);
            int i = ptmService.EditPlanTypeMaster(model);
            return i.ToString();
        }
        public string DeletePlanTypeMaster(int plantypemasterID)
        {
            PlanTypeMaster plan = new PlanTypeMaster();
            plan.PlanTypeMasterID = plantypemasterID;
            PlanTypeMasterService ptmService = new PlanTypeMasterService();
            int i = ptmService.DeletePlanTypeMaster(plan);
            return i.ToString();
        }
        public string GetPlanTypeMasterByID(int planTypeMasterID)
        {
            PlanTypeMasterService ptmService = new PlanTypeMasterService();
            return Newtonsoft.Json.JsonConvert.SerializeObject(ptmService.GetPlanTypeMasterByID(planTypeMasterID));
        }
        public string GetPlanTypeMasterList()
        {
            PlanTypeMasterService ptmService = new PlanTypeMasterService();
            return Newtonsoft.Json.JsonConvert.SerializeObject(ptmService.GetPlanTypeMasterList());
        }
        public string GetPlanTypeMasterDistList()
        {
            PlanTypeMasterService ptmService = new PlanTypeMasterService();
            return Newtonsoft.Json.JsonConvert.SerializeObject(ptmService.GetPlanTypeDistModel());
        }
        #endregion

        #region Plan Service
        public string SavePlan(PlanModel pln)
        {
            PlanService ptmService = new PlanService();
            Mapper.CreateMap<PlanModel, Plan>();
            var model = Mapper.Map<PlanModel, Plan>(pln);
            int i = ptmService.AddPlan(model);
            return i.ToString();
        }
        public string EditPlan(PlanModel pln)
        {
            PlanService ptmService = new PlanService();
            Mapper.CreateMap<PlanModel, Plan>();
            var model = Mapper.Map<PlanModel, Plan>(pln);
            int i = ptmService.EditPlan(model);
            return i.ToString();
        }
        public string DeletePlan(int planid)
        {
            PlanService ptmService = new PlanService();
            Mapper.CreateMap<PlanModel, Plan>();
            Plan plan = new Plan();
            plan.PlanID = planid;
            int i = ptmService.DeletePlan(plan);
            return i.ToString();
        }
        public string GetPlanByID(int planID)
        {
            PlanService ptmService = new PlanService();
            return Newtonsoft.Json.JsonConvert.SerializeObject(ptmService.GetPlanByID(planID));
        }
        public string GetPlanList()
        {
            PlanService ptmService = new PlanService();
            return Newtonsoft.Json.JsonConvert.SerializeObject(ptmService.GetPlanList());
        }
        #endregion
    }
}