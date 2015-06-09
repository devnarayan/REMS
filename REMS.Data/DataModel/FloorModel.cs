using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
   public class FloorModel
    {
        public int FloorID { get; set; }
        public Nullable<int> TowerID { get; set; }
        public string TowerNo { get; set; }
        public string TowerName { get; set; }
        public string Block { get; set; }
        public Nullable<int> FloorNo { get; set; }
        public string FloorName { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public Nullable<int> NoOfFlat { get; set; }
    }
}
