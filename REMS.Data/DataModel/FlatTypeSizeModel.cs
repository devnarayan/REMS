using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class FlatTypeSizeModel
    {
        public int FlatTypeSizeID { get; set; }
        public Nullable<int> FlatTypeID { get; set; }
        public Nullable<decimal> Size { get; set; }
        public string Unit { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string FType { get; set; }
    }
}
