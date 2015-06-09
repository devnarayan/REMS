using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class RefundPropertyModel
    {
        public int RefundPropertyID { get; set; }
        public int SaleID { get; set; }
        public System.DateTime RefundDate { get; set; }
        public decimal RefundAmount { get; set; }
        public string PaymentMode { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Remarks { get; set; }
        public string FlatName { get; set; }
        public string RefundDateSt { get; set; }
        public string ChequeDateSt { get; set; }
        public string SaleRate { get; set; }
    }
}
