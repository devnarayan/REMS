using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class PlanModel
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
