using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class AssuredReturnModel
    {
        public int AssuredReturnID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public string MonthName { get; set; }
        public string Duration { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<int> PropertyID { get; set; }
        public string FlatName { get; set; }
        public string CrDateSt { get; set; }
        public string PayStatus { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string ChequeDateSt { get; set; }
        public string UnClearStatus { get; set; }

    }
}