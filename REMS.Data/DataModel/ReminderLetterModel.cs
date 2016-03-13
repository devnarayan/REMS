using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class ReminderLetterModel
    {
        public int ID { get; set; }
        public string LetterType { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string LetterDateSt { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string DueDateST { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public string DueAmountSt { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TowerNo { get; set; }
        public string FlatNo { get; set; }
        public string ProjectName { get; set; }
        public string PropertyLocation { get; set; }
        public string PropertyAddress { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactNo { get; set; }
        public string AuthorityPerson { get; set; }
        public string AuthEmail { get; set; }
        public string BSPPending { get; set; }
        public string PLCPending { get; set; }
        public string STaxPending { get; set; }
        public string OtherPending { get; set; }

        public string InterestRate { get; set; }
        public string Letter2Date { get; set; }
        public string Letter3Date { get; set; }
    }
}