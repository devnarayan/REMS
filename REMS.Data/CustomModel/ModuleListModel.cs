using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class ModuleListModel
    {
        public int ModuleListID { get; set; }

        public string Name { get; set; }

        public string PageName { get; set; }

        public bool Status { get; set; }

        public Nullable<System.DateTime> CrDate { get; set; }

        public string Controller { get; set; }

        public string ActionName { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public string Checked { get; set; }

    }
}
