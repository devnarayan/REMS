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
    
    public partial class UploadAgreement
    {
        public int UploadAgreementID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public string FilePath { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public string ModifyDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual SaleFlat SaleFlat { get; set; }
    }
}
