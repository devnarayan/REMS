//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddOnCharge
    {
        public AddOnCharge()
        {
            this.FlatOCharges = new HashSet<FlatOCharge>();
        }
    
        public int AddOnChargeID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string ChargeType { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<bool> Mandatory { get; set; }
    
        public virtual ICollection<FlatOCharge> FlatOCharges { get; set; }
    }
}
