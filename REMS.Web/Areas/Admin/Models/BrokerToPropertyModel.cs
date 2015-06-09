using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class BrokerToPropertyModel
    {
        public int BrokerToPropertyID { get; set; }

        public Nullable<int> FlatID { get; set; }

        public Nullable<int> SaleID { get; set; }

        public Nullable<int> BrokerID { get; set; }

        public Nullable<decimal> BrokerAmount { get; set; }

        public Nullable<int> PID { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public Nullable<System.DateTime> Createdate { get; set; }

        public Nullable<System.DateTime> ModifyDate { get; set; }

        public Nullable<int> RecordStatus { get; set; }

        public Nullable<int> ApproveStatus { get; set; }

        public string Remarks { get; set; }
        public string DateSt { get; set; }
        public string BrokerName { get; set; }
        public string PropertyName { get; set; }
        public string Status { get; set; }
    }
}