using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public  class TowerViewModel
    {
        public int FloorID { get; set; }
        public Nullable<int> FloorNo { get; set; }
        public string FloorName { get; set; }
        public int TowerID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ProjectTypeID { get; set; }
        public string TowerName { get; set; }
        public string TowerNo { get; set; }
        public string Block { get; set; }
        public string PName { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string ReceiptPrefix { get; set; }
        public string PossessionDateSt { get; set; }
        public List<Flat> FlatList { get; set; }

    }
}
