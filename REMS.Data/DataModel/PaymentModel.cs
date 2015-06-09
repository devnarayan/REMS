﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Data.DataModel
{
    public class PaymentModel
    {
        public Nullable<long> TransactionID { get; set; }
        public string InstallmentNo { get; set; }
        public string PaymentNo { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> ServiceTaxAmount { get; set; }
        public Nullable<decimal> InterestAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaymentMode { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string PaymentStatus { get; set; }
        public string CustomerName { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> BankCharges { get; set; }
        public Nullable<System.DateTime> BankClearanceDate { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public string BankBranch { get; set; }
        public string AmtRcvdinWords { get; set; }
        public Nullable<bool> IsReceipt { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string FlatName { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public string isrefund { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public int PaymentID { get; set; }
        public string ModifyBy { get; set; }
        public string CreatedBy { get; set; }
        public string RefundRemark { get; set; }
        public decimal ClearanceCharge { get; set; }
        public bool IsBounce { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string PaymentDateSt { get; set; }
        public string ChequeDateSt { get; set; }
        public string BankClearanceDateSt { get; set; }
    }
}