using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class PlanTypeMasterModel
    {
        public int PlanTypeMasterID { get; set; }
        public Nullable<int> PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<decimal> AmountSqFt { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<int> FlatTypeSizeID { get; set; }
        public string FType { get; set; }
        public Nullable<decimal> Size { get; set; }
        public string PlanSection { get; set; }

    }
}
