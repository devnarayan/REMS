using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class ProjectTypeModel
    {
        public int ProjectTypeID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public string TypeName { get; set; }
    }
}
