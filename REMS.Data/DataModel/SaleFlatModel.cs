using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
   public class SaleFlatModel
    {
        public int SaleID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public string Aggrement { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public string SaleRateInWords { get; set; }
        public Nullable<decimal> ServiceTaxPer { get; set; }
        public Nullable<decimal> ServiceTaxAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string TotalAmtInWords { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> DemandStatus { get; set; }
    }
}
