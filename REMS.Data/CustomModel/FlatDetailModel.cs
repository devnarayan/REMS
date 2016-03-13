using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class FlatDetailModel
    {
        public int FlatID { get; set; }
        public int FloorID { get; set; }
        public string FlatType { get; set; }
        public string FlatName { get; set; }
        public string FlatNo { get; set; }
        public Nullable<decimal> FlatSize { get; set; }
        public string FlatSizeUnit { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<int> BadRooms { get; set; }
        public Nullable<bool> IsFurnished { get; set; }
        public Nullable<bool> LivingRoom { get; set; }
        public Nullable<bool> ServantRoom { get; set; }
        public Nullable<bool> Kitchen { get; set; }
        public Nullable<bool> Balconies { get; set; }
        public Nullable<bool> BathRooms { get; set; }
        public Nullable<bool> PoojaRoom { get; set; }
        public Nullable<bool> StudyRoom { get; set; }
        public Nullable<bool> AC { get; set; }
        public Nullable<bool> Intercom { get; set; }
        public Nullable<bool> Cupboards { get; set; }
        public Nullable<bool> WashingArea { get; set; }
        public Nullable<bool> PowerBackup { get; set; }
        public Nullable<bool> GASLine { get; set; }
        public Nullable<bool> HomeAutomation { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public string Status { get; set; }
        public int TowerID { get; set; }
        public string TowerName { get; set; }
        public string TowerNo { get; set; }
        public string Block { get; set; }
        public Nullable<int> FloorNo { get; set; }
        public string FloorName { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public List<FlatPLCModel> FlatPLCList { get; set; }
        public List<FlatChargeModel> FlatChargeList { get; set; }
        public List<Rem_GetFlatPlanCharge_Result> FlatPlanCharge { get; set; }
        public ObjectResult<spPlanSummary_Result> ChargeSummaryList { get; set; }
        public List<FlatOChargeModel> FlatOChargeList { get; set; }
        public List<SaleFlatModel> SaleFlatModel { get; set; }
    }
}
