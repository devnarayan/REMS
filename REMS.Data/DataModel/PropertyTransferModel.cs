﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
   public class PropertyTransferModel
    {
        public int PropertyTransferID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<int> OldCustomerID { get; set; }
        public Nullable<int> NewCustomerID { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<decimal> TransferAmount { get; set; }
        public string OldPlanType { get; set; }
        public string NewPlanType { get; set; }
        public string UserName { get; set; }
        public string TransferDateSt { get; set; }
    }
}
