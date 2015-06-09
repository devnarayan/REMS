using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
   public class UserAccessModel
    {
        public int UserAccessID { get; set; }
        public string ModuleListID { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public string AssignUser { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
