using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class ReminderLetterModel2
    {

        public int ReminderLetterID { get; set; }
        public string LetterType { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<System.DateTime> duedate { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> BSPTotal { get; set; }
        public Nullable<decimal> BSPPaid { get; set; }
        public Nullable<decimal> PLCTotal { get; set; }
        public Nullable<decimal> PLCPaid { get; set; }
        public Nullable<decimal> ACTotal { get; set; }
        public Nullable<decimal> ACPaid { get; set; }
        public Nullable<decimal> AOCTotal { get; set; }
        public Nullable<decimal> AOCPaid { get; set; }
        public Nullable<decimal> STaxTotal { get; set; }
        public Nullable<decimal> STaxPaid { get; set; }
        public Nullable<decimal> LateTotal { get; set; }
        public Nullable<decimal> LatePaid { get; set; }
        public Nullable<decimal> TransferTotal { get; set; }
        public Nullable<decimal> TransferPaid { get; set; }
        public Nullable<decimal> ClearanceTotal { get; set; }
        public Nullable<decimal> ClearancePaid { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> FlatID { get; set; }
        
        public string DueDateSt { get; set; }
        public string CrDateSt { get; set; }
        public string CustomerName { get; set; }
        public string FlatNo { get; set; }
    }
}
