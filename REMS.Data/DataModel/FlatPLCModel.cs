using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class FlatPLCModel
    {
        public int FlatPLCID { get; set; }
        public int FlatID { get; set; }
        public int PLCID { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string PLCName { get; set; }
        public Nullable<decimal> AmountSqFt { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    
    
    }
}
