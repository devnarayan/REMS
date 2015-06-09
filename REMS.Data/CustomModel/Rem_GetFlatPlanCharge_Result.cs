using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class Rem_GetFlatPlanCharge_Result
    {
        public int PlanID { get; set; }
        public string FlatName { get; set; }
        public string FullName { get; set; }
        public string PlanName { get; set; }
        public Nullable<decimal> AmountSqFt { get; set; }
        public Nullable<decimal> Size { get; set; }
    }
}
