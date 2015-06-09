using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Data.DataModel
{
    public class EventModel
    {
        public int InstallmentID { get; set; }
        public string InstallmentNo { get; set; }
        public Nullable<int> FlatID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public string DueDate { get; set; }
        public Nullable<int> EventID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string ModifyDate { get; set; }
        public string EventName { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }


        public Nullable<int> PropertyTypeID { get; set; }
        public Nullable<int> PropertySizeID { get; set; }
    }
}