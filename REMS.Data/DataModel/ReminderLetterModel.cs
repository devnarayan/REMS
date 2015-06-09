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

        public Nullable<int> SaleID { get; set; }

        public Nullable<System.DateTime> duedate { get; set; }

        public Nullable<decimal> DueAmount { get; set; }

        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string PropertyAddress { get; set; }
        public string LetterDateSt { get; set; }
        public string FlatName { get; set; }
        public string DueDateST { get; set; }
        public string InterestRate { get; set; }
        public string CompanyName { get; set; }
        public string Letter2Date { get; set; }
        public string Letter3Date { get; set; }
    }
}
