using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class FollowupModel
    {
        public int Rid { get; set; }

        public Nullable<int> SaleID { get; set; }

        public string Remark { get; set; }

        public Nullable<System.DateTime> RemakDate { get; set; }

        public Nullable<int> status { get; set; }

        public string ProprtyName { get; set; }

        public Nullable<System.DateTime> createdate { get; set; }

        public Nullable<decimal> FollowupAmount { get; set; }
        public string FlatName { get; set; }
        public string RemarkDateSt { get; set; }
        public string CreatedBy { get; set; }


    }
}