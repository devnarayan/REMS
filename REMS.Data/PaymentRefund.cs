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
    
    public partial class PaymentRefund
    {
        public int RefundID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> RefundAmount { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string BankName { get; set; }
        public Nullable<System.DateTime> RefundDate { get; set; }
        public Nullable<bool> IsPan { get; set; }
        public Nullable<bool> IsPhoto { get; set; }
        public Nullable<bool> IsAddressPf { get; set; }
        public Nullable<bool> IsRationCard { get; set; }
        public Nullable<bool> IsDrivingLicence { get; set; }
        public Nullable<bool> IsPassport { get; set; }
        public Nullable<bool> IsVoterCard { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public string Remarks { get; set; }
    
        public virtual SaleFlat SaleFlat { get; set; }
    }
}
