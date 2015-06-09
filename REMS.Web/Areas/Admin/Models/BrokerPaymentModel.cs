using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class BrokerPaymentModel
    {
        public int BrokerPaymentID { get; set; }

        public Nullable<int> FlatID { get; set; }

        public Nullable<int> SaleID { get; set; }

        public Nullable<int> BrokerID { get; set; }

        public Nullable<decimal> AmountPaid { get; set; }

        public Nullable<System.DateTime> PaidDate { get; set; }

        public Nullable<int> PID { get; set; }

        public Nullable<System.DateTime> CreateDate { get; set; }

        public Nullable<System.DateTime> ModifyDate { get; set; }

        public Nullable<int> RecordStatus { get; set; }

        public string PaymentMode { get; set; }

        public string ChequeNo { get; set; }

        public Nullable<System.DateTime> ChequeDate { get; set; }

        public string BankName { get; set; }

        public string BankBranch { get; set; }

        public string Remarks { get; set; }
        public string ChequeDateSt { get; set; }
        public string PaymentDateSt { get; set; }
        public string PropertyName { get; set; }
        public string Status { get; set; }
    }
}