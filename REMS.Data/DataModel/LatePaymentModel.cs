using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
  public class LatePaymentModel
    {
        public int LatePaymentID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<int> TransactionID { get; set; }
        public string InstallmentNo { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<decimal> InterestAmount { get; set; }
        public Nullable<decimal> ReceiveAmount { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string DueDateSt { get; set; }
        public string CrDateSt { get; set; }
    }
}
