using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Data.CustomModel
{
    public class FlatDetail
    {
        public string FlatName { get; set; }
        public string BlockName { get; set; }
        public string SaleAmount { get; set; }
        public string FlatID { get; set; }
        public string CustomerID { get; set; }
        public string FlatNo { get; set; }
        public string SaleDate { get; set; }
        public string SalePrice { get; set; }
        public string SaleRate { get; set; }
        public string InstallmentCount { get; set; }
        public string PLCPrice { set; get; }
        public string CustomerName { set; get; }

        
        public string IsPaymentDetails { set; get; }

       public FlatDetail()
        {
            PaymentInstallmentList paymentInstallmentList = new PaymentInstallmentList();
        }

       public PaymentInstallmentList paymentInstallmentList { set; get; }
    }

    public class PaymentInstallment {
        public int InstallmentID { set; get; }
        public string InstallmentNumber { set; get; }
        public string DueDate { set; get;}
        public string DueAmount { set; get; }
        public int InstallmentNo {set;get;}
        
        
        public string  TotalAmount {set;get;}
        public int PaymentMode{set;get;}
        public int ChequeNo{set;get;}
        public string BankName {set;get;}
        public string BankBranch{set;get;}
        public string Remarks{set;get;}
    }

    public class PaymentInstallmentList : List<PaymentInstallment>
    { }

    public class BankDetails {
        public int BankID { set; get; }
        public string BankName { set; get; }
    }
}