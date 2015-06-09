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
    public class FlatController : Controller
    {
        // GET: Master/Flat
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FlatSize()
        {
            return View();
        }
        #region Manage FlatType and FlatTypeSize Services
        FlatTypeService ftservice = new FlatTypeService();
        FlatTypeSizeService ftsservice = new FlatTypeSizeService();
        public string GetFlatType()
        {
            var model = ftservice.GetFlatTypeList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatTypeByID(int flattypeid)
        {
            var model = ftservice.GetFlatTypeByID(flattypeid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string SaveFlatType(FlatTypeModel ftp)
        {
            Mapper.CreateMap<FlatTypeModel, FlatType>();
            var model = Mapper.Map<FlatTypeModel, FlatType>(ftp);
            if (model.IsFurnished == null) model.IsFurnished = false;
            int i = ftservice.AddFlatType(model);
            return i.ToString();
        }
        public string UpdateFlatType(FlatTypeModel ftp)
        {
            Mapper.CreateMap<FlatTypeModel, FlatType>();
            var model = Mapper.Map<FlatTypeModel, FlatType>(ftp);
            int i = ftservice.EditFlatType(model);
            return i.ToString();
        }
        public string GetFlatTypeSizeList(int falttypeid)
        {
            var model = ftsservice.GetFlatTypeSizeListByFlattypeid(falttypeid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatTypeSizeByNameList(string ftype)
        {
            var model = ftsservice.GetFlatTypeSizeListByFlattypeName(ftype);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetFlatTypeSizeByID(int falttypesizeid)
        {
            var model = ftsservice.GetFlatTypeSizeByID(falttypesizeid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        public string GetFlatTypeSizeByPID(int flattypeSizeid)
        {
            var model = ftsservice.GetFlatTypeSizeByID(flattypeSizeid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string SaveFlatTypeSize(FlatTypeSizeModel ftps)
        {
            if (ftps.Size == null)
            {
                return "2";
            }
            else
            {
                Mapper.CreateMap<FlatTypeSizeModel, FlatTypeSize>();
                var model = Mapper.Map<FlatTypeSizeModel, FlatTypeSize>(ftps);
                int i = ftsservice.AddFlatTypeSize(model);
                return i.ToString();
            }
        }
        public string UpdateFlatTypeSize(FlatTypeSizeModel ftps)
        {
            Mapper.CreateMap<FlatTypeSizeModel, FlatTypeSize>();
            var model = Mapper.Map<FlatTypeSizeModel, FlatTypeSize>(ftps);
            int i = ftsservice.EditFlatTypeSize(model);
            return i.ToString();
        }
        #endregion
    }
}