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
    
    public partial class FlatOCharge
    {
        public int FlatOChargeID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public Nullable<int> AddOnChargeID { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
    
        public virtual AddOnCharge AddOnCharge { get; set; }
        public virtual Flat Flat { get; set; }
    }
}