using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class BrokerModel
    {
        public int BrokerID { get; set; }

        public string BrokerName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string MobileNo { get; set; }

        public string PanNo { get; set; }

        public Nullable<System.DateTime> CreateDate { get; set; }

        public Nullable<System.DateTime> ModifyDate { get; set; }

        public Nullable<int> RecordStatus { get; set; }
        public string CreateDateSt { get; set; }
    }
}