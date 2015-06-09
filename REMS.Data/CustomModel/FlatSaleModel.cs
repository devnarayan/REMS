using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.CustomModel
{
    public class FlatSaleModel
    {
        public int SaleID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public string Aggrement { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public string SaleRateInWords { get; set; }
        public Nullable<decimal> ServiceTaxPer { get; set; }
        public Nullable<decimal> ServiceTaxAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string TotalAmtInWords { get; set; }
        public Nullable<int> Installments { get; set; }
        public Nullable<int> Interval { get; set; }
        public Nullable<decimal> BookingAmount { get; set; }
        public Nullable<decimal> PossessionAmount { get; set; }
        public string AppTitle { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Title { get; set; }
        public string PName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PAN { get; set; }
        public string MobileNo { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string CoAppTitle { get; set; }
        public string CoFName { get; set; }
        public string CoMName { get; set; }
        public string CoLName { get; set; }
        public string CoAddress1 { get; set; }
        public string CoAddress2 { get; set; }
        public string CoCity { get; set; }
        public string CoState { get; set; }
        public string CoCountry { get; set; }
        public string CoPAN { get; set; }
        public string CoMobileNo { get; set; }
        public string Address1Out { get; set; }
        public string Address2Out { get; set; }
        public string CityOut { get; set; }
        public string StateOut { get; set; }
        public string CountryOut { get; set; }
        public string PANOut { get; set; }
        public string MobileOut { get; set; }
        public string AlternateMobile { get; set; }
        public string LandLine { get; set; }
        public string EmailID { get; set; }
        public string AlternateEmail { get; set; }
        public string Website { get; set; }
        public string BrokerName { get; set; }
        public Nullable<decimal> BrokerAmount { get; set; }
        public string BrokerAddress1 { get; set; }
        public string BrokerAddress2 { get; set; }
        public string BrokerMobile { get; set; }
        public string BrokerPAN { get; set; }
        public Nullable<decimal> LoanAmount { get; set; }
        public string LienField { get; set; }
        public Nullable<int> BankID { get; set; }
        public string BankAddress { get; set; }
        public string BankBranch { get; set; }
        public Nullable<bool> IsPAN { get; set; }
        public Nullable<bool> IsPhoto { get; set; }
        public Nullable<bool> IsAddressPf { get; set; }
        public Nullable<bool> IsRationCard { get; set; }
        public Nullable<bool> IsDrivingLicence { get; set; }
        public Nullable<bool> IsPassport { get; set; }
        public Nullable<bool> IsVoterCard { get; set; }
        public Nullable<int> IsTransfer { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public Nullable<int> PlanID { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public Nullable<int> ApproveStatus { get; set; }
        public string SecCoAppTitle { get; set; }
        public string SecCoFName { get; set; }
        public string SecCoMName { get; set; }
        public string SecCoLName { get; set; }
        public string SecCoAddress1 { get; set; }
        public string SecCoAddress2 { get; set; }
        public string SecCoCity { get; set; }
        public string SecCoState { get; set; }
        public string SecCoCountry { get; set; }
        public string SecCoMobileNo { get; set; }
        public string SecCoPAN { get; set; }
        public string CoTitle { get; set; }
        public string CoPName { get; set; }
        public string SecCoTitle { get; set; }
        public string SecCoPName { get; set; }
        public Nullable<int> IsConstruction { get; set; }
        public string PhotoImagePath { get; set; }
        public string AddPfImagePath { get; set; }
        public string DVImagePath { get; set; }
        public string VoterCardImagePath { get; set; }
        public string PassportImagePath { get; set; }
        public string RationCardImagePath { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CoDOB { get; set; }
        public Nullable<System.DateTime> SecCoDOB { get; set; }
        public string PANImagePath { get; set; }
        public Nullable<bool> CoIsPAN { get; set; }
        public Nullable<bool> CoIsPhoto { get; set; }
        public Nullable<bool> CoIsAddressPf { get; set; }
        public Nullable<bool> CoIsRationCard { get; set; }
        public Nullable<bool> CoIsDrivingLicence { get; set; }
        public Nullable<bool> CoIsVoterCard { get; set; }
        public Nullable<bool> CoIsPassport { get; set; }
        public Nullable<bool> SecCoIsPAN { get; set; }
        public Nullable<bool> SecCoIsPhoto { get; set; }
        public Nullable<bool> SecCoIsAddressPf { get; set; }
        public Nullable<bool> SecCoIsRationCard { get; set; }
        public Nullable<bool> SecCoIsDrivingLicence { get; set; }
        public Nullable<bool> SecCoIsVoterCard { get; set; }
        public Nullable<bool> SecCoIsPassport { get; set; }
        public string CoPhotoImagePath { get; set; }
        public string CoAddPfImagePath { get; set; }
        public string CoDVImagePath { get; set; }
        public string CoVoterCardImagePath { get; set; }
        public string CoPassportImagePath { get; set; }
        public string CoRationCardImagePath { get; set; }
        public string CoPANImagePath { get; set; }
        public string SecCoPhotoImagePath { get; set; }
        public string SecCoAddPfImagePath { get; set; }
        public string SecCoDVImagePath { get; set; }
        public string SecCoVoterCardImagePath { get; set; }
        public string SecCoPassportImagePath { get; set; }
        public string SecCoRationCardImagePath { get; set; }
        public string SecCoPANImagePath { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public string PinCode { get; set; }
        public string ExecutiveName { get; set; }
        public string CoPinCode { get; set; }
        public string SecCoPinCode { get; set; }
        public Nullable<decimal> TransferAmount { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<int> IsUpload { get; set; }
        public string Distt { get; set; }
        public string LegalOptionDate { get; set; }
        public Nullable<int> panallowstatus { get; set; }
        public string affidavit { get; set; }
        public Nullable<int> PropertyID { get; set; }
        public string PaymentFor { get; set; }
        public string FlatName { get; set; }
        public Nullable<System.DateTime> BookingDate { get; set; }
        public Nullable<int> PropertyTypeID { get; set; }
        public Nullable<int> PropertySizeID { get; set; }
        public string BookingDateSt { get; set; }
        public string SaleDateSt { get; set; }

        public string Statusrecord { get; set; }


        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }

        public Nullable<decimal> DueAmount { get; set; }

        public string InstallmentID { get; set; }

        public string InstallmentNo { get; set; }

        public string DueDateST { get; set; }

        public string FtRDateSt { get; set; }
        public string StRDateSt { get; set; }
        public string StRStatus { get; set; }
        public string CreateBy { get; set; }
        public string Rid { get; set; }
        public Nullable<decimal> Dueamount { get; set; }
        public string EventName { get; set; }
        public string propertySatuts { get; set; }
    }
}
