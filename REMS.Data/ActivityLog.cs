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
    
    public partial class ActivityLog
    {
        public int ActivityLogID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string Message { get; set; }
    
        public virtual Flat Flat { get; set; }
    }
}
