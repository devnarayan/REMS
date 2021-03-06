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
    
    public partial class RefundProperty
    {
        public int RefundPropertyID { get; set; }
        public int SaleID { get; set; }
        public System.DateTime RefundDate { get; set; }
        public decimal RefundAmount { get; set; }
        public string PaymentMode { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Remarks { get; set; }
        public string FlatName { get; set; }
    
        public virtual SaleFlat SaleFlat { get; set; }
    }
}
