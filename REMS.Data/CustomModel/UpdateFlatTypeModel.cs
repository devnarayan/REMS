using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
   public class UpdateFlatTypeModel
    {
      public int TowerID { get; set; }
      public int FloorID { get; set; }
      public string FlatType { get; set; }
       public Nullable<decimal> FlatSize { get; set; }
    }
}
