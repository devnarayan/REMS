using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class PaymentCancelModel
    {
        public int PaymentCancelID { get; set; }
        public Nullable<long> TransactionID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public Nullable<System.DateTime> UnCancelDate { get; set; }
        public string Remarks { get; set; }
        public string UnCancelRemark { get; set; }
        public string CancelBy { get; set; }
        public string UnCancelBy { get; set; }
        public string Status { get; set; }
        public string FlatName { get; set; }
        public string CustomerName { get; set; }
        public string CancelDateSt { get; set; }
        public string UnCancelDateSt { get; set; }
    }
}
