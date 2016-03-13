using AutoMapper;
using REMS.Data.Access.Master;
using REMS.Data.Access.Sale;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Admin
{
    public interface IFlatService
    {
        int AddFloor(int towerid, int florno, string username);
        int AddFlats(int TotalFloor, int FlatOrder, List<int> FlatNo, List<bool> PreIncrement, List<PLCModel> pmodel, string username, int towerID, string FltType, string FltTSize);
        string FlatCreateBody();
        int AddNewFlat(Flat flat);
        int EditFlat(Flat flat,string userName);
        int EditFlatStatus(int flatid, string status);
        int DeleteFlat(int flatid);
        FlatModel GetFlatByID(int flatid);
        List<FlatModel> GetFlatsByFloorID(int floorid);
        bool CheckFlatNo(string flatno, int towerID);
        int UpdateFlatTypeAllTowerPerFloor(UpdateFlatTypeModel model, string userName);
        int UpdateFlatTypePerTowerAllFloor(UpdateFlatTypeModel model, string userName);
        int UpdateFlatTypePerTowerPerFloor(UpdateFlatTypeModel model,string userName);

        FlatDetailModel GetFlatDetails(int FlatID);
        List<FlatModel> GetFlatListByTowerID(int towerid);
        List<FlatModel> GetReservedFlatListByTowerID(int towerid);
        List<FlatPLCModel> GetFlatPLCList(int flatID);
        List<FlatOChargeModel> GetFlatOChargeList(int flatid);
        List<FlatChargeModel> GetFlatChargeList(int flatid);
    }
    public class FlatService :SaleFlatService, IFlatService
    {
        public int AddFloor(int towerID, int florno, string username)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    for (int i = 1; i <= florno; i++)
                    {
                        //Floor fl = new Floor();
                        //fl.CrBy = username; fl.CrDate = DateTime.Now;
                        //fl.TowerID = towerID;
                        //fl.FloorName = i.ToString();
                        //fl.FloorNo = i;
                        //context.Floors.Add(fl);
                        //context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }
        public int AddFlats(int TotalFloor, int FlatOrder, List<int> FlatNo, List<bool> PreIncrement, List<PLCModel> pmodel, string username, int towerID, string FltType, string FltTSize)
        {
            PLCService pservice = new PLCService();
            AdditionalChargeService adsService = new AdditionalChargeService();
            AOChargeService aoService = new AOChargeService();
            FlatChargeService fcService = new FlatChargeService();
            FlatOChargeService focService = new FlatOChargeService();
            int plcCount = pservice.GetMandatoryPLCCount();
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    for (int i = 1; i <= TotalFloor; i++)
                    {
                        int kk = 0;
                        Floor flo = new Floor();
                        flo.CrBy = username; flo.CrDate = DateTime.Now;
                        flo.TowerID = towerID;
                        flo.FloorName = i.ToString();
                        flo.FloorNo = i;
                        context.Floors.Add(flo);
                        context.SaveChanges();
                        int jj = 0;
                        foreach (int flat in FlatNo)
                        {
                            int fno = GenFlatNo(i, flat, PreIncrement[jj]);
                            Flat fl = new Flat();
                            fl.FlatOrder = FlatOrder;
                            fl.FlatNo = fno.ToString();
                            fl.FlatName = fno.ToString();
                            fl.FloorID = flo.FloorID;
                            fl.FlatType = FltType;
                            fl.FlatSizeUnit = "SqFt";
                            fl.FlatSize = Convert.ToDecimal(FltTSize);
                            fl.Status = "Available";
                            context.Flats.Add(fl);
                            context.SaveChanges();

                            // adding mandatory plc
                            for (int k = 0; k <= plcCount - 1; k++)
                            {
                                var pl = pmodel[kk];
                                if (pl.PLCID != 0)
                                {
                                    FlatPLC fpcl = new FlatPLC();
                                    fpcl.CrDate = DateTime.Now;
                                    fpcl.FlatID = fl.FlatID;
                                    fpcl.PLCID = pl.PLCID;
                                    context.FlatPLCs.Add(fpcl);
                                    context.SaveChanges();
                                }
                                kk++;
                            }
                            // adding floor plc
                            var floorplc = pservice.GetFloorPLC();
                            foreach (var md in floorplc)
                            {
                                if (md.FloorNo == i)
                                {
                                    FlatPLC fpcl = new FlatPLC();
                                    fpcl.CrDate = DateTime.Now;
                                    fpcl.FlatID = fl.FlatID;
                                    fpcl.PLCID = md.PLCID;
                                    context.FlatPLCs.Add(fpcl);
                                    context.SaveChanges();
                                }
                            }
                            // Adding mandatory AdditionalCharge
                            var adchargemodel = adsService.GetMandatoryAdditionalChargeList();
                            foreach (var md in adchargemodel)
                            {
                                FlatCharge ft = new FlatCharge();
                                ft.AdditionalChargeID = md.AdditionalChargeID;
                                ft.FlatID = fl.FlatID;
                                ft.CrDate = DateTime.Now;
                                fcService.AddFlatCharge(ft);
                            }
                            var aochargemodel = aoService.GetMandatoryAddOnChargeList();
                            foreach (var md in aochargemodel)
                            {
                                FlatOCharge fot = new FlatOCharge();
                                fot.AddOnChargeID = md.AddOnChargeID;
                                fot.FlatID = fl.FlatID;
                                fot.CrDate = DateTime.Now;
                                focService.AddFlatOCharge(fot);
                            }
                            jj++;
                        }

                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        private int GenFlatNo(int floorno, int flatno, bool increment)
        {
            int pre = Convert.ToInt32(flatno.ToString().Substring(0, 1));
            int preRem = Convert.ToInt32(flatno.ToString().Substring(1, flatno.ToString().Length - 1));

            int post = Convert.ToInt32(flatno.ToString().Substring(flatno.ToString().Length - 1, 1));
            int postRem = Convert.ToInt32(flatno.ToString().Substring(0, flatno.ToString().Length - 1));

            if (increment)
            {
                return Convert.ToInt32((pre + floorno - 1).ToString() + "" + preRem);
            }
            else
            {
                return Convert.ToInt32(postRem.ToString() + "" + (post + floorno - 1).ToString());
            }
        }
        public string FlatCreateBody()
        {
            String htmlText = @"  
                <div class='col col-md-3'>
                    <label class='input'>
                        <input type='text' name='FlatNo' id='<% FlatNo %>' placeholder='First Floor Flat No'>
                    </label><br />
                    <label class='checkbox'>
                        <input type='checkbox' name='PreIncrement' id='<% PreIncrement %>' checked='checked'>
                        <i></i>Pre-Increment
                    </label>
                    <b>PLC Options</b><br />";
            return htmlText;
        }

        public int AddNewFlat(Flat flat)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Flats.Add(flat);
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }

        public int EditFlat(Flat flat,string userName)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Flats.Add(flat);
                    context.Entry(flat).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    // Update SaleFlat Status
                    UpdateFlatSaleStatus(flat.FlatID,userName, "Regenerate");
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public int EditFlatStatus(int flatid, string status)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    var fmodel= context.Flats.Where(fl => fl.FlatID == flatid).FirstOrDefault();
                    fmodel.Status = status;
                    context.Flats.Add(fmodel);
                    context.Entry(fmodel).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }

        public int DeleteFlat(int flatid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    FlatPLCService fpservice = new FlatPLCService();
                    FlatChargeService fcservice = new FlatChargeService();
                    var model = fpservice.GetFlatPLCListByFlatID(flatid);
                    foreach (var md in model)
                    {
                        FlatPLC plc = new FlatPLC();
                        plc.FlatPLCID = md.FlatPLCID;
                        context.FlatPLCs.Add(plc);
                        context.Entry(plc).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    var fmodel = fcservice.GetFlatChargeListByFlatID(flatid);
                    foreach (var md in fmodel)
                    {
                        FlatCharge plc = new FlatCharge();
                        plc.FlatChargeID = md.FlatChargeID;
                        context.FlatCharges.Add(plc);
                        context.Entry(plc).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    Flat ft = new Flat();
                    ft.FlatID = flatid;
                    context.Flats.Add(ft);
                    context.Entry(ft).State = EntityState.Deleted;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }

        public FlatModel GetFlatByID(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Flats.Where(fl => fl.FlatID == flatid).FirstOrDefault();
           
            Mapper.CreateMap<Flat, FlatModel>();
            var md = Mapper.Map<Flat, FlatModel>(model);
            md.FloorName= model.Floor.FloorName;
            md.ProjectName = model.Floor.Tower.Project.PName;
            md.CompanyName = model.Floor.Tower.Project.CompanyName;
            md.ProjectID = model.Floor.Tower.ProjectID;
            return md;
        }
        public List<FlatModel> GetFlatsByFloorID(int floorid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Flats.Where(fl => fl.FloorID == floorid).ToList();
            Mapper.CreateMap<Flat, FlatModel>();
            var md = Mapper.Map<List<Flat>, List<FlatModel>>(model);
            return md;
        }
        public bool CheckFlatNo(string flatno, int towerID)
        {
            return false;
        }

        public int UpdateFlatTypeAllTowerPerFloor(UpdateFlatTypeModel model,string userName)
        {
            TowerService ts = new TowerService();
            FloorService fs = new FloorService();

            var alltower = ts.AllTower();
            foreach (Tower tw in alltower)
            {
                Floor fd = fs.GetFloorByFloorNo(tw.TowerID, model.FloorID);
                var flats = GetFlatsByFloorID(fd.FloorID);
                foreach (var flm in flats)
                {
                    var flat = GetFlatByID(flm.FlatID);
                    flat.FlatSize = model.FlatSize;
                    flat.FlatType = model.FlatType;
                    Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, gest => gest.Ignore());
                    var flt = Mapper.Map<FlatModel, Flat>(flat);
                    EditFlat(flt,userName);
                }
            }
            return 1;
        }
        public int UpdateFlatTypePerTowerAllFloor(UpdateFlatTypeModel model,string userName)
        {
            TowerService ts = new TowerService();
            FloorService fs = new FloorService();

            var allfloor = fs.AllFloor(model.TowerID);
            foreach (var tw in allfloor)
            {
                var flats = GetFlatsByFloorID(tw.FloorID);
                foreach (var flm in flats)
                {
                    var flat = GetFlatByID(flm.FlatID);
                    flat.FlatSize = model.FlatSize;
                    flat.FlatType = model.FlatType;
                    Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, gest => gest.Ignore());
                    var flt = Mapper.Map<FlatModel, Flat>(flat);
                    EditFlat(flt,userName);
                }
            }
            return 1;
        }
        public int UpdateFlatTypePerTowerPerFloor(UpdateFlatTypeModel model,string userName)
        {
            var flats = GetFlatsByFloorID(model.FloorID);
            foreach (var flm in flats)
            {
                var flat = GetFlatByID(flm.FlatID);
                flat.FlatSize = model.FlatSize;
                flat.FlatType = model.FlatType;
                Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, gest => gest.Ignore());
                var flt = Mapper.Map<FlatModel, Flat>(flat);
                EditFlat(flt,userName);
            }
            return 1;
        }

        //public FlatDetailModel GetFlatDetails(int FlatID)
        //{
        //    REMSDBEntities context = new REMSDBEntities();
        //    var flmodel = context.Flats.Where(fl => fl.FlatID == FlatID).FirstOrDefault();
        //    Mapper.CreateMap<Flat, FlatDetailModel>();
        //    FlatDetailModel model = new FlatDetailModel();
        //    model = Mapper.Map<Flat, FlatDetailModel>(flmodel);
        //    var flor = context.Floors.Where(fl => fl.FloorID == model.FloorID).FirstOrDefault();
        //    model.FloorName = flor.FloorName;
        //    model.FloorNo = flor.FloorNo;
        //    var twr = context.Towers.Where(tw => tw.TowerID == flor.TowerID).FirstOrDefault();
        //    model.TowerID = twr.TowerID;
        //    model.TowerName = twr.TowerName;
        //    model.TowerNo = twr.TowerNo;
        //    model.Block = twr.Block;
        //    var fmodel = context.FlatPLCs.Where(pl => pl.FlatID == FlatID).ToList();
        //    List<FlatPLCModel> FlatPLCList = new List<FlatPLCModel>();
        //    foreach (FlatPLC fp in fmodel)
        //    {
        //        Mapper.CreateMap<FlatPLC, FlatPLCModel>();
        //        var fpm = Mapper.Map<FlatPLC, FlatPLCModel>(fp);
        //        fpm.PLCName = fp.PLC.PLCName;
        //        fpm.AmountSqFt = fp.PLC.AmountSqFt;
        //        fpm.TotalAmount = fp.PLC.AmountSqFt * model.FlatSize;
        //        FlatPLCList.Add(fpm);
        //    }
        //    model.FlatPLCList = FlatPLCList;
        //    var amodel = context.FlatCharges.Where(fc => fc.FlatID == FlatID).ToList();
        //    List<FlatChargeModel> FlatChargeList = new List<FlatChargeModel>();
        //    foreach (var ac in amodel)
        //    {
        //        Mapper.CreateMap<FlatCharge, FlatChargeModel>();
        //        var acmodel = Mapper.Map<FlatCharge, FlatChargeModel>(ac);
        //        acmodel.ChargeName = ac.AdditionalCharge.Name;
        //        acmodel.Amount = ac.AdditionalCharge.Amount;
        //        acmodel.ChargeType = ac.AdditionalCharge.ChargeType;
        //        if (acmodel.ChargeType == "Free")
        //        {
        //            acmodel.TotalAmount = 0;
        //        }
        //        else if (acmodel.ChargeType == "Sq. Ft.")
        //        {
        //            acmodel.TotalAmount = ac.AdditionalCharge.Amount * model.FlatSize;
        //        }
        //        else if (acmodel.ChargeType == "One Time")
        //        {
        //            acmodel.TotalAmount = ac.AdditionalCharge.Amount;
        //        }
        //        FlatChargeList.Add(acmodel);
        //    }
        //    model.FlatChargeList = FlatChargeList;
        //    return model;
        //}

        //public FlatDetailModel GetFlatDetails(int FlatID)
        //{
        //    REMSDBEntities context = new REMSDBEntities();
        //    var flmodel = context.Flats.Where(fl => fl.FlatID == FlatID).FirstOrDefault();
        //    Mapper.CreateMap<Flat, FlatDetailModel>();
        //    FlatDetailModel model = new FlatDetailModel();
        //    model = Mapper.Map<Flat, FlatDetailModel>(flmodel);
        //    var flor = context.Floors.Where(fl => fl.FloorID == model.FloorID).FirstOrDefault();
        //    model.FloorName = flor.FloorName;
        //    model.FloorNo = flor.FloorNo;
        //    var twr = context.Towers.Where(tw => tw.TowerID == flor.TowerID).FirstOrDefault();
        //    model.TowerID = twr.TowerID;
        //    model.TowerName = twr.TowerName;
        //    model.TowerNo = twr.TowerNo;
        //    model.Block = twr.Block;
        //    var fmodel = context.FlatPLCs.Where(pl => pl.FlatID == FlatID).ToList();
        //    List<FlatPLCModel> FlatPLCList = new List<FlatPLCModel>();
        //    foreach (FlatPLC fp in fmodel)
        //    {
        //        Mapper.CreateMap<FlatPLC, FlatPLCModel>();
        //        var fpm = Mapper.Map<FlatPLC, FlatPLCModel>(fp);
        //        fpm.PLCName = fp.PLC.PLCName;
        //        fpm.AmountSqFt = fp.PLC.AmountSqFt;
        //        fpm.TotalAmount = fp.PLC.AmountSqFt * model.FlatSize;
        //        FlatPLCList.Add(fpm);
        //    }
        //    model.FlatPLCList = FlatPLCList;
        //    var amodel = context.FlatCharges.Where(fc => fc.FlatID == FlatID).ToList();
        //    List<FlatChargeModel> FlatChargeList = new List<FlatChargeModel>();
        //    foreach (var ac in amodel)
        //    {
        //        Mapper.CreateMap<FlatCharge, FlatChargeModel>();
        //        var acmodel = Mapper.Map<FlatCharge, FlatChargeModel>(ac);
        //        acmodel.ChargeName = ac.AdditionalCharge.Name;
        //        acmodel.Amount = ac.AdditionalCharge.Amount;
        //        acmodel.ChargeType = ac.AdditionalCharge.ChargeType;
        //        if (acmodel.ChargeType == "Free")
        //        {
        //            acmodel.TotalAmount = 0;
        //        }
        //        else if (acmodel.ChargeType == "Sq. Ft.")
        //        {
        //            acmodel.TotalAmount = ac.AdditionalCharge.Amount * model.FlatSize;
        //        }
        //        else if (acmodel.ChargeType == "One Time")
        //        {
        //            acmodel.TotalAmount = ac.AdditionalCharge.Amount;
        //        }
        //        FlatChargeList.Add(acmodel);
        //    }
        //    model.FlatPlanCharge= context.Rem_GetFlatPlanCharge(FlatID).ToList();
        //    var flatOCModel = context.FlatOCharges.Where(p => p.FlatID == FlatID).ToList();
        //    List<FlatOChargeModel> fomodel = new List<FlatOChargeModel>();
        //    foreach (var md in flatOCModel)
        //    {
        //        Mapper.CreateMap<FlatOCharge, FlatOChargeModel>();
        //        var mdl= Mapper.Map<FlatOCharge, FlatOChargeModel>(md);
        //        mdl.ChargeName = md.AddOnCharge.Name;
        //        mdl.ChargeType = md.AddOnCharge.ChargeType;
        //        mdl.Amount = md.AddOnCharge.Amount;
        //        decimal? tamt = 0;
        //        if (md.AddOnCharge.ChargeType == "Free")
        //        {
        //            tamt = 0;
        //        }
        //        else if (md.AddOnCharge.ChargeType == "Sq. Ft.")
        //        {
        //            tamt =md.AddOnCharge.Amount * model.FlatSize;
        //        }
        //        else if (md.AddOnCharge.ChargeType == "One Time")
        //        {
        //           tamt = md.AddOnCharge.Amount;
        //        }
        //        mdl.TotalAmount = tamt;
        //        fomodel.Add(mdl);
        //    }
        //    model.FlatOChargeList = fomodel;

        //    model.FlatChargeList = FlatChargeList;

        //    return model;
        //}

        public FlatDetailModel GetFlatDetails(int FlatID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var flmodel = context.Flats.Where(fl => fl.FlatID == FlatID).FirstOrDefault();
            Mapper.CreateMap<Flat, FlatDetailModel>();
            FlatDetailModel model = new FlatDetailModel();
            model = Mapper.Map<Flat, FlatDetailModel>(flmodel);
            model.FlatSize = flmodel.FlatSize;
            model.FlatSizeUnit = flmodel.FlatSizeUnit;
            model.SalePrice = flmodel.SalePrice;
            // Floor
            var flor = context.Floors.Where(fl => fl.FloorID == model.FloorID).FirstOrDefault();
            model.FloorName = flor.FloorName;
            model.FloorNo = flor.FloorNo;
            // Tower
            var twr = context.Towers.Where(tw => tw.TowerID == flor.TowerID).FirstOrDefault();
            model.TowerID = twr.TowerID;
            model.TowerName = twr.TowerName;
            model.TowerNo = twr.TowerNo;
            model.Block = twr.Block;
            model.ProjectName = twr.Project.PName;
            model.CompanyName = twr.Project.CompanyName;
           
            // Sale Flat
            var flatSale = context.SaleFlats.Where(fl => fl.FlatID == FlatID).ToList();
            List<SaleFlatModel> fSaleModel = new List<SaleFlatModel>();
            foreach (SaleFlat sale in flatSale)
            {
                Mapper.CreateMap<SaleFlat, SaleFlatModel>().ForMember(ds => ds.SaleDateSt, sc => sc.MapFrom(ds => ds.SaleDate.Value.ToString("dd/MM/yyyy")));
                var mdl = Mapper.Map<SaleFlat, SaleFlatModel>(sale);
                fSaleModel.Add(mdl);
            }
            model.SaleFlatModel=fSaleModel;
            // Flat PLC
            var fmodel = context.FlatPLCs.Where(pl => pl.FlatID == FlatID).ToList();
            List<FlatPLCModel> FlatPLCList = new List<FlatPLCModel>();
            foreach (FlatPLC fp in fmodel)
            {
                Mapper.CreateMap<FlatPLC, FlatPLCModel>();
                var fpm = Mapper.Map<FlatPLC, FlatPLCModel>(fp);
                fpm.PLCName = fp.PLC.PLCName;
                fpm.AmountSqFt = fp.PLC.AmountSqFt;
                fpm.TotalAmount = fp.PLC.AmountSqFt * model.FlatSize;
                FlatPLCList.Add(fpm);
            }
            model.FlatPLCList = FlatPLCList;
            // Flat Charge
            var amodel = context.FlatCharges.Where(fc => fc.FlatID == FlatID).ToList();
            List<FlatChargeModel> FlatChargeList = new List<FlatChargeModel>();
            foreach (var ac in amodel)
            {
                Mapper.CreateMap<FlatCharge, FlatChargeModel>();
                var acmodel = Mapper.Map<FlatCharge, FlatChargeModel>(ac);
                acmodel.ChargeName = ac.AdditionalCharge.Name;
                acmodel.Amount = ac.AdditionalCharge.Amount;
                acmodel.ChargeType = ac.AdditionalCharge.ChargeType;
                if (acmodel.ChargeType == "Free")
                {
                    acmodel.TotalAmount = 0;
                }
                else if (acmodel.ChargeType == "Sq. Ft.")
                {
                    acmodel.TotalAmount = ac.AdditionalCharge.Amount * model.FlatSize;
                }
                else if (acmodel.ChargeType == "One Time")
                {
                    acmodel.TotalAmount = ac.AdditionalCharge.Amount;
                }
                FlatChargeList.Add(acmodel);
            }
            // Flat Plan Charge
            model.FlatPlanCharge = context.Rem_GetFlatPlanCharge(FlatID).ToList();
            // Plan Sumary
            model.ChargeSummaryList = context.spPlanSummary(model.FlatSize);
            // Flat Other Charge
            var flatOCModel = context.FlatOCharges.Where(p => p.FlatID == FlatID).ToList();
            List<FlatOChargeModel> fomodel = new List<FlatOChargeModel>();
            foreach (var md in flatOCModel)
            {
                Mapper.CreateMap<FlatOCharge, FlatOChargeModel>();
                var mdl = Mapper.Map<FlatOCharge, FlatOChargeModel>(md);
                mdl.ChargeName = md.AddOnCharge.Name;
                mdl.ChargeType = md.AddOnCharge.ChargeType;
                mdl.Amount = md.AddOnCharge.Amount;
                decimal? tamt = 0;
                if (md.AddOnCharge.ChargeType == "Free")
                {
                    tamt = 0;
                }
                else if (md.AddOnCharge.ChargeType == "Sq. Ft.")
                {
                    tamt = md.AddOnCharge.Amount * model.FlatSize;
                }
                else if (md.AddOnCharge.ChargeType == "One Time")
                {
                    tamt = md.AddOnCharge.Amount;
                }
                mdl.TotalAmount = tamt;
                fomodel.Add(mdl);
            }
            model.FlatOChargeList = fomodel;

            model.FlatChargeList = FlatChargeList;

            return model;
        }
        public List<FlatModel> GetFlatListByTowerID(int towerid)
        {
            REMSDBEntities dbContext = new REMSDBEntities();
            var model= dbContext.Flats.Where(fl => fl.Floor.TowerID == towerid).ToList();
            Mapper.CreateMap<Flat, FlatModel>();
            var mdl= Mapper.Map<List<Flat>, List<FlatModel>>(model);
            return mdl;
        }
        public List<FlatModel> GetReservedFlatListByTowerID(int towerid)
        {
            REMSDBEntities dbContext = new REMSDBEntities();
            var model = dbContext.Flats.Where(fl => fl.Floor.TowerID == towerid && fl.Status != "Available").ToList();
            Mapper.CreateMap<Flat, FlatModel>();
            var mdl = Mapper.Map<List<Flat>, List<FlatModel>>(model);
            return mdl;
        }

        public List<FlatPLCModel> GetFlatPLCList(int flatID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var flmodel = context.Flats.Where(fl => fl.FlatID == flatID).FirstOrDefault();

            var fmodel = context.FlatPLCs.Where(pl => pl.FlatID == flatID).ToList();
            List<FlatPLCModel> FlatPLCList = new List<FlatPLCModel>();
            foreach (FlatPLC fp in fmodel)
            {
                Mapper.CreateMap<FlatPLC, FlatPLCModel>();
                var fpm = Mapper.Map<FlatPLC, FlatPLCModel>(fp);
                fpm.PLCName = fp.PLC.PLCName;
                fpm.AmountSqFt = fp.PLC.AmountSqFt;
                fpm.TotalAmount = fp.PLC.AmountSqFt * flmodel.FlatSize;
                FlatPLCList.Add(fpm);
            }
            return FlatPLCList;
        }
        public List<FlatOChargeModel> GetFlatOChargeList(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var flmodel = context.Flats.Where(fl => fl.FlatID == flatid).FirstOrDefault();
            var flatOCModel = context.FlatOCharges.Where(p => p.FlatID == flatid).ToList();
            List<FlatOChargeModel> fomodel = new List<FlatOChargeModel>();
            foreach (var md in flatOCModel)
            {
                Mapper.CreateMap<FlatOCharge, FlatOChargeModel>();
                var mdl = Mapper.Map<FlatOCharge, FlatOChargeModel>(md);
                mdl.ChargeName = md.AddOnCharge.Name;
                mdl.ChargeType = md.AddOnCharge.ChargeType;
                mdl.Amount = md.AddOnCharge.Amount;
                decimal? tamt = 0;
                if (md.AddOnCharge.ChargeType == "Free")
                {
                    tamt = 0;
                }
                else if (md.AddOnCharge.ChargeType == "Sq. Ft.")
                {
                    tamt = md.AddOnCharge.Amount * flmodel.FlatSize;
                }
                else if (md.AddOnCharge.ChargeType == "One Time")
                {
                    tamt = md.AddOnCharge.Amount;
                }
                mdl.TotalAmount = tamt;
                fomodel.Add(mdl);
            }
            return fomodel;
        }
        public List<FlatChargeModel> GetFlatChargeList(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();
            // Flat Charge
            var flmodel = context.Flats.Where(fl => fl.FlatID == flatid).FirstOrDefault();

            var amodel = context.FlatCharges.Where(fc => fc.FlatID == flatid).ToList();
            List<FlatChargeModel> FlatChargeList = new List<FlatChargeModel>();
            foreach (var ac in amodel)
            {
                Mapper.CreateMap<FlatCharge, FlatChargeModel>();
                var acmodel = Mapper.Map<FlatCharge, FlatChargeModel>(ac);
                acmodel.ChargeName = ac.AdditionalCharge.Name;
                acmodel.Amount = ac.AdditionalCharge.Amount;
                acmodel.ChargeType = ac.AdditionalCharge.ChargeType;
                if (acmodel.ChargeType == "Free")
                {
                    acmodel.TotalAmount = 0;
                }
                else if (acmodel.ChargeType == "Sq. Ft.")
                {
                    acmodel.TotalAmount = ac.AdditionalCharge.Amount * flmodel.FlatSize;
                }
                else if (acmodel.ChargeType == "One Time")
                {
                    acmodel.TotalAmount = ac.AdditionalCharge.Amount;
                }
                FlatChargeList.Add(acmodel);
            }
            return FlatChargeList;
        }
    }
}