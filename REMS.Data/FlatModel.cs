using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data
{
   public class FlatModel
    {
        public int FlatID { get; set; }
        public int FloorID { get; set; }
        public string FlatType { get; set; }    
        public string FlatName { get; set; }
        public string FlatNo { get; set; }
        public Nullable<decimal> FlatSize { get; set; }
        public string FlatSizeUnit { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<int> BadRooms { get; set; }
        public Nullable<bool> IsFurnished { get; set; }
        public Nullable<bool> LivingRoom { get; set; }
        public Nullable<bool> ServantRoom { get; set; }
        public Nullable<bool> Kitchen { get; set; }
        public Nullable<bool> Balconies { get; set; }
        public Nullable<bool> BathRooms { get; set; }
        public Nullable<bool> PoojaRoom { get; set; }
        public Nullable<bool> StudyRoom { get; set; }
        public Nullable<bool> AC { get; set; }
        public Nullable<bool> Intercom { get; set; }
        public Nullable<bool> Cupboards { get; set; }
        public Nullable<bool> WashingArea { get; set; }
        public Nullable<bool> PowerBackup { get; set; }
        public Nullable<bool> GASLine { get; set; }
        public Nullable<bool> HomeAutomation { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public string Status { get; set; }
        public string[] FlatPLCs { get; set; }
    }
}
