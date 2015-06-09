using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace REMS.Web.App_Helpers
{
    public class DataHelper
    {
        public static DateTimeFormatInfo IndianDateFormat()
        {
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            return dtinfo;
        }
    }
}