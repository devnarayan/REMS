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
    
    public partial class PropertyRemark
    {
        public int Rid { get; set; }
        public Nullable<int> SaleID { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> RemakDate { get; set; }
        public Nullable<int> status { get; set; }
        public string ProprtyName { get; set; }
        public Nullable<System.DateTime> createdate { get; set; }
        public Nullable<decimal> FollowupAmount { get; set; }
        public string CreatedBy { get; set; }
    
        public virtual SaleFlat SaleFlat { get; set; }
    }
}
