using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class FlatTypeModel
    {
        public int FlatTypeID { get; set; }
        public string FType { get; set; }
        public string FullName { get; set; }
        public Nullable<decimal> Size { get; set; }
        public Nullable<bool> IsFurnished { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
    }
}
