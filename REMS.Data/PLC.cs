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
    
    public partial class PLC
    {
        public PLC()
        {
            this.FlatPLCs = new HashSet<FlatPLC>();
        }
    
        public int PLCID { get; set; }
        public string PLCName { get; set; }
        public Nullable<decimal> AmountSqFt { get; set; }
        public Nullable<int> FloorNo { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<bool> Mandatory { get; set; }
    
        public virtual ICollection<FlatPLC> FlatPLCs { get; set; }
    }
}
