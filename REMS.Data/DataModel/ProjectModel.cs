using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public string PName { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string ReceiptPrefix { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string Tehsil { get; set; }
        public string Jurisdiction { get; set; }
        public Nullable<System.DateTime> PossessionDate { get; set; }
        public string ReceiptNo { get; set; }
        public string AuthoritySign { get; set; }
        public string PossessionDateSt { get; set; }

    }
}
