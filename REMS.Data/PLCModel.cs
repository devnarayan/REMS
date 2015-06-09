using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data
{
    public class PLCModel
    {
        public int PLCID { get; set; }
        public string PLCName { get; set; }
        public Nullable<decimal> AmountSqFt { get; set; }
        public Nullable<int> FloorNo { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<bool> Mandatory { get; set; }
    }
}
