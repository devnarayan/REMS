using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class PlanInstallmentModel
    {
        public int PlanInstallmentID { get; set; }
        public Nullable<int> PlanID { get; set; }
        public string Installment { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public Nullable<int> IntervalDays { get; set; }
    }
}
