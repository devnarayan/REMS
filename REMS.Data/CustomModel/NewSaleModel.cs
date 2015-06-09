using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class NewSaleModel
    {
        public int? FlatsID { get; set; }
        public string Installment { get; set; }
        public string bsp { get; set; }
        public string plc { get; set; }
        public string acharges { get; set; }
        public string ocharges { get; set; }
        public string dueDate { get; set; }
        public decimal? InstallmentTotal { get; set; }
        public decimal? PLCTotal { get; set; }
        public decimal? AChargeTotal { get; set; }
        public decimal? OChargeTotal { get; set; }
    }
}
