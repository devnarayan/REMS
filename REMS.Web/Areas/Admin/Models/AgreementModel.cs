using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class AgreementModel
    {
        public int AgreementID { get; set; }

        public Nullable<int> SaleID { get; set; }

        public string AgreementAmount { get; set; }

        public string CreatedBy { get; set; }

        public string ModifyBy { get; set; }

        public Nullable<System.DateTime> CrDate { get; set; }

        public Nullable<System.DateTime> ModifyDate { get; set; }

        public string DocURL { get; set; }
        public string HTMLURL { get; set; }
        public Nullable<System.DateTime> UploadDate { get; set; }

        public string UploadBy { get; set; }

        public string UploadURL { get; set; }

        public string AllotmentLetter { get; set; }

        public string AllotmentLetterBy { get; set; }

        public Nullable<System.DateTime> AllotmentLetterDate { get; set; }

        public string WelComeLetter { get; set; }
        public string UploadDateSt { get; set; }

        public string CrDateSt { get; set; }
        public string AllotmentLetterDateSt { get; set; }
        public string AssuredHTMLURL { get; set; }
        public string AssuredDocURL { get; set; }
    }
}