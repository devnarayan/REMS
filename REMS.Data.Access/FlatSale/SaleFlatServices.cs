using REMS.Data.CustomModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.FlatSale
{
    interface ISaleFlatServices
    {

    }
    class SaleFlatServices
    {
        public List<Project> GetProperty()
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                int Uid = 1;
                return context.Projects.ToList();
            }
        }
        public List<FlatDetail> GetSaleDetailByFlatId(Int64 flatId)
        {
            REMSDBEntities context = new REMSDBEntities();
            List<FlatDetail> l_FlatDetail = new List<FlatDetail>();
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            var saleFlats = from ft in context.Flats
                            join sale in context.SaleFlats on ft.FlatID equals sale.FlatID
                            join
                                cr in context.Customers on sale.SaleID equals cr.SaleID
                            select new { ft.FlatName, cr.FName, cr.LName, sale.SaleDate };
            var installMentDetail = context.FlatInstallmentDetails.Where(p => p.SaleFlat.FlatID == flatId).ToList();
            l_FlatDetail.Add(new FlatDetail { SaleDate = saleFlats.ToList()[0].SaleDate.ToString(), CustomerName = saleFlats.ToList()[0].FName + " " + saleFlats.ToList()[0].LName, FlatNo = saleFlats.ToList()[0].FlatName });
            if (installMentDetail.Count > 0)
            {
                l_FlatDetail.Add(new FlatDetail { IsPaymentDetails = "Yes" });
                PaymentInstallmentList paymentList = new PaymentInstallmentList();
                for (int i = 0; i < installMentDetail.Count - 1; i++)
                {
                    paymentList.Add(new PaymentInstallment { TotalAmount = installMentDetail[i].TotalAmount.ToString(), DueAmount = installMentDetail[i].TotalAmount.ToString(), DueDate = installMentDetail[i].DueDate.ToString(), InstallmentID = installMentDetail[i].InstallmentID, InstallmentNumber = installMentDetail[i].InstallmentID.ToString() });

                }
                l_FlatDetail.Add(new FlatDetail { paymentInstallmentList = paymentList });
            }


            return l_FlatDetail;
        }
        public string GetInstallmetnDetailsBySaleId(int saleId, string planType, string saleRate)
        {
            REMSDBEntities context = new REMSDBEntities();
            StringBuilder l_StringBuilder = new StringBuilder();
            var list = context.FlatInstallmentDetails.Where(p => p.SaleID == saleId).ToList();
            decimal bspPercentage = 0;
            if (planType == "1")
            {

                l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                l_StringBuilder.Append("<thead><tr><th>InstallmentNo</th><th>Due Date</th><th>Due Amount</th></tr></thead><tbody>");
                if (list.Count > 0)
                {
                    decimal dueAmount;
                    string dueDate = String.Empty;
                    decimal TotalAmount = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        dueAmount = decimal.Parse(list[i].TotalAmount.ToString());
                        if (!String.IsNullOrEmpty(Convert.ToString(list[i].DueDate)))
                        {
                            l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td></td><td>" + list[i].DueDate + "</td><td>" + dueAmount + "</td><tr>");
                        }
                        else
                        {
                            l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td></td><td>" + dueDate + "</td><td>" + dueAmount + "</td><tr>");
                        }
                        TotalAmount = TotalAmount + dueAmount;
                    }
                    l_StringBuilder.Append("<tr><td></td></td><td>Total</td><td><b>" + TotalAmount + "</b></td><tr>");
                }
                l_StringBuilder.Append("</tbody>");
            }
            else if (planType == "2")
            {
                l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                l_StringBuilder.Append("<thead><tr><th>InstallmentNo</th><th>Due Date</th><th>Due Amount</th></tr></thead><tbody>");
                if (list.Count > 0)
                {
                    decimal dueAmount;
                    decimal TotalAmount = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        dueAmount = decimal.Parse(list[i].TotalAmount.ToString());
                        bspPercentage = decimal.Parse(saleRate) * decimal.Parse(list[i].SaleFlat.Flat.FlatSize.ToString());
                        dueAmount += bspPercentage;
                        if (!String.IsNullOrEmpty(Convert.ToString(list[i].DueDate)))
                        {
                            l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td></td><td>" + list[i].DueDate + "</td><td>" + dueAmount + "</td><tr>");
                        }
                        else
                        {
                            l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td></td><td></td><td>" + dueAmount + "</td><tr>");
                        }
                        TotalAmount = TotalAmount + dueAmount;
                    }
                    l_StringBuilder.Append("<tr><td></td></td><td> Total</td><td><b>" + TotalAmount + "</b></td><tr>");
                }
                l_StringBuilder.Append("</tbody>");
            }
            else if (planType == "4" || planType == "5")
            {
                //var comboPlan = ComboList();
                l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                l_StringBuilder.Append("<thead><tr><th>InstallmentNo</th><th>Due Date</th><th>Due Amount</th></tr></thead><tbody>");
                if (list.Count > 0)
                {
                    decimal dueAmount;
                    decimal TotalAmount = 0;
                    //  decimal bspPercentage;
                    for (int i = 0; i < list.Count; i++)
                    {
                        dueAmount = decimal.Parse(list[i].TotalAmount.ToString());
                        if (!String.IsNullOrEmpty(Convert.ToString(list[i].DueDate)))
                        {
                            l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td></td><td>" + list[i].DueDate.ToString() + "</td><td>" + dueAmount + "</td><tr>");
                        }
                        else
                        {
                            l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td></td><td></td><td>" + dueAmount + "</td><tr>");
                        }
                        TotalAmount = TotalAmount + dueAmount;
                    }
                    l_StringBuilder.Append("<tr><td></td></td><td>Total</td><td><b>" + TotalAmount + "</b></td><tr>");
                }
                l_StringBuilder.Append("</tbody>");
            }
            else
            {
                l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                l_StringBuilder.Append("<thead><tr><th>INstallmet No</th><th>Due Date</th><th>%</th><th>Amount</th></tr></thead><tbody>");
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        l_StringBuilder.Append("<tr><td>" + list[i].InstallmentID + "</td><td>" + list[i].DueDate.Value.ToString("dd/MM/yyyy") + "</td><td>" + bspPercentage + "</td><td>" + list[i].TotalAmount + "</td><tr>");
                    }
                }
                l_StringBuilder.Append("</tbody>");
            }
            return Convert.ToString(l_StringBuilder);
        }

        public void SaveInstallments(string saledate, string saleprice, string salepriceword, int ddlInstallment, int ddlInterval, string eventName, string bspPercentage, string saleId, string flatId, string evName, string amount, string pplPrice)
        {

            REMSDBEntities context = new REMSDBEntities();
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.DateSeparator = "/";
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            DateTime dt2 = DateTime.Now;
            if (saledate != "")
                dt2 = Convert.ToDateTime(saledate, dtinfo);


            var st = context.SaleFlats.Where(s => s.SaleID == Convert.ToInt32(saleId)).FirstOrDefault<SaleFlat>();
            st.SaleRate = Convert.ToDecimal(saleprice);
            st.SaleRateInWords = salepriceword;
            st.SaleDate = dt2;
            st.Status = "1";
            context.SaveChanges();

            StringBuilder l_StringBuilder = new StringBuilder();
            string PlanType = "";
            amount = amount.Replace("undefined,", "");
            amount = amount.TrimEnd(',');
            string[] amounts = amount.Split(',');
            evName = evName.Replace("undefined,", "");
            evName = evName.TrimEnd(',');
            string[] EvNames = evName.Split(',');
            string[] bspPer = bspPercentage.Split(',');
            var fsz = context.Flats.Where(s => s.FlatID == Convert.ToInt32(flatId)).ToList()[0].FlatSize;
            decimal.Parse(saleprice) * decimal.Parse(fsz.ToString());
            if (PlanType == "1")
            {
                DataTable tb = new DataTable();
                if (Session["table"] != null)
                {
                    tb = Session["table"] as DataTable;
                }
                if (tb.Rows.Count > 0)
                {
                    string[] EventIdd = EventName.Split(',');
                    int j = 0;
                    for (int a = 0; a < tb.Rows.Count; a++)
                    {
                        string InstallmentID = Convert.ToString(tb.Rows[a]["InstallmentID"]);
                        string InstallmentNo = Convert.ToString(tb.Rows[a]["InstallmentNo"]);
                        if (a > 0)
                        {
                            InstallmentNo = a.ToString();
                        }
                        string DueAmount = Convert.ToString(tb.Rows[a]["DueAmount"]);
                        string EventID = Convert.ToString(tb.Rows[a]["EventID"]);
                        string TotalAmount = Convert.ToString(tb.Rows[a]["TotalAmount"]);
                        if (DueAmount != "")
                        {
                            Hashtable ht = new Hashtable();
                            ht.Add("DueAmount", DueAmount);
                            ht.Add("DueAmtInWords", clsnew.rupees(Convert.ToInt64(DueAmount)));

                            ht.Add("TotalAmount", saleprice);
                            ht.Add("TotalAmtInWords", clsnew.rupees(Convert.ToInt64(saleprice)));
                            ht.Add("SaleID", Convert.ToInt32(Session["SaleID"].ToString()));
                            ht.Add("InstallmentNo", InstallmentNo);
                            ht.Add("EventName", InstallmentNo);

                            if (!String.IsNullOrEmpty(Convert.ToString(tb.Rows[a]["DueDate"])))
                            {
                                ht.Add("DueDate", obj.Text_IndianDateFormat(Convert.ToString(tb.Rows[a]["DueDate"])));
                            }
                            ht.Add("FlatID", Convert.ToInt32(Session["FlatID"].ToString()));
                            if (Session["TowerID"] != null)
                            {
                                ht.Add("TowerID", Convert.ToInt32(Session["TowerID"].ToString()));
                            }
                            if (InstallmentNo == "Booking Amount" || InstallmentNo == "Possession Amount")
                            {
                                ht.Add("Type", 0);
                            }
                            else
                            {
                                j++;
                                ht.Add("Type", 1);
                            }
                            ht.Add("Activity", "ADD");
                            if (obj.ExecuteProcedure("InsertInstallmentDetail", ht))
                            {
                            }
                        }
                    }
                }
            }
            else if (PlanType == "2")
            {
                DataTable tb = new DataTable();
                #region Construction Based Plan

                if (Session["table"] != null)
                {

                    tb = Session["table"] as DataTable;
                }
                if (tb.Rows.Count > 0)
                {
                    string[] EventIdd = EventName.Split(',');
                    int j = 0;
                    for (int a = 0; a < tb.Rows.Count; a++)
                    {
                        string InstallmentID = Convert.ToString(tb.Rows[a]["InstallmentID"]);
                        string InstallmentNo = Convert.ToString(tb.Rows[a]["InstallmentNo"]);
                        string DueAmount = Convert.ToString(tb.Rows[a]["DueAmount"]);
                        string EventID = Convert.ToString(tb.Rows[a]["EventID"]);
                        string TotalAmount = Convert.ToString(tb.Rows[a]["TotalAmount"]);
                        string DueDate = Convert.ToString(tb.Rows[a]["DueDate"]);
                        if (DueAmount != "")
                        {
                            Hashtable ht = new Hashtable();
                            ht.Add("DueAmount", amounts[a]);
                            ht.Add("DueAmtInWords", clsnew.rupees(Convert.ToDecimal(amounts[a])));

                            ht.Add("TotalAmount", amounts[a]);
                            ht.Add("TotalAmtInWords", clsnew.rupees(Convert.ToDecimal(amounts[a])));
                            ht.Add("SaleID", Convert.ToInt32(Session["SaleID"].ToString()));

                            if (DueDate != "")
                            {
                                ht.Add("DueDate", obj.Text_IndianDateFormat(Convert.ToString(tb.Rows[a]["DueDate"])));           /* Balkrishna  Add one Line By */
                            }

                            ht.Add("FlatID", Convert.ToInt32(Session["FlatID"].ToString()));
                            if (Session["TowerID"] != null)
                            {
                                ht.Add("TowerID", Convert.ToInt32(Session["TowerID"].ToString()));
                            }
                            if (InstallmentNo.Contains("Booking Amount") || InstallmentNo.Contains("Possession Amount"))
                            {
                                ht.Add("Type", 0);
                                ht.Add("InstallmentNo", InstallmentNo);
                                ht.Add("EventName", InstallmentNo);
                            }
                            else
                            {
                                if (!String.IsNullOrWhiteSpace(EventIdd[j]))
                                { ht.Add("EventID", Convert.ToInt32(EventIdd[j])); }
                                ht.Add("EventName", EvNames[j]);
                                ht.Add("InstallmentNo", EvNames[j]);
                                j++;
                                ht.Add("Type", 1);
                            }
                            ht.Add("Activity", "ADD");
                            ht.Add("Bsp", bspPer[a]);
                            if (obj.ExecuteProcedure("[InsertInstallmentDetail]", ht))
                            {
                            }
                        }
                    }
                }
                #endregion
            }
            else if (PlanType == "3")
            {
                #region Normal Mode
                DataTable dt = new DataTable();
                if (Session["table"] != null)
                    dt = (DataTable)Session["table"];
                //string EventName="";
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string InstallmentID = Convert.ToString(a + 1);// Convert.ToString(dt.Rows[a]["InstallmentID"]);
                    EventName = Convert.ToString(dt.Rows[a]["Event"]);
                    string EventID = "0";
                    string InstallmentNo = Convert.ToString(dt.Rows[a]["Event"]);// Convert.ToString(dt.Rows[a]["InstallmentNo"]);
                    string DueDate = Convert.ToString(dt.Rows[a]["DueDate"]);
                    string DueAmount = Convert.ToString(dt.Rows[a]["Amount"]);
                    if (DueAmount != "")
                    {
                        Hashtable ht = new Hashtable();
                        if (DueDate != "")
                        {
                            ht.Add("DueDate", Convert.ToDateTime(DueDate, dtinfo));
                        }

                        ht.Add("DueAmount", amounts[a]);
                        ht.Add("DueAmtInWords", clsnew.rupees(Convert.ToInt64(amounts[a])));
                        // ht.Add("ServiceTaxAmount",ServiceTaxAmount.Text);
                        ht.Add("TotalAmount", amounts[a]);
                        ht.Add("TotalAmtInWords", clsnew.rupees(Convert.ToInt64(amounts[a])));
                        ht.Add("SaleID", Convert.ToInt32(Session["SaleID"].ToString()));
                        ht.Add("InstallmentNo", InstallmentNo);
                        ht.Add("FlatID", Convert.ToInt32(Session["FlatID"].ToString()));
                        ht.Add("Type", 0);
                        ht.Add("UserID", Convert.ToInt32("0"));
                        ht.Add("Activity", "Add");
                        ht.Add("EventName", EventName);
                        ht.Add("EventID", EventID);
                        ht.Add("FloorNo", 0);
                        ht.Add("TowerID", 0);
                        ht.Add("BID", 0);
                        ht.Add("BSP", bspPer[a]);
                        if (obj.ExecuteProcedure("InsertInstallmentDetailCLP", ht))
                        {
                        }
                    }
                }
                #endregion
            }
            else if (PlanType == "4" || PlanType == "5")
            {
                DataTable tb = new DataTable();
                #region Combo Plan

                if (Session["table"] != null)
                {
                    tb = Session["table"] as DataTable;
                }
                if (tb.Rows.Count > 0)
                {
                    string[] EventIdd = EventName.Split(',');
                    int j = 0;
                    for (int a = 0; a < tb.Rows.Count; a++)
                    {
                        string InstallmentID = Convert.ToString(tb.Rows[a]["InstallmentID"]);
                        string InstallmentNo = Convert.ToString(tb.Rows[a]["InstallmentNo"]);
                        string DueAmount = Convert.ToString(tb.Rows[a]["DueAmount"]);
                        string EventID = Convert.ToString(tb.Rows[a]["EventID"]);
                        string Eventorder = Convert.ToString(tb.Rows[a]["EventID"]);
                        string DueDate = "";
                        string TotalAmount = Convert.ToString(tb.Rows[a]["TotalAmount"]);
                        if (DueAmount != "")
                        {
                            Hashtable ht = new Hashtable();
                            if (InstallmentNo.Contains("Booking Amount"))
                            {
                                ht.Add("DueDate", obj.Text_IndianDateFormat(saledate));
                            }
                            if (DueDate != "")
                            {
                                ht.Add("DueDate", obj.Text_IndianDateFormat(DueDate));
                            }
                            ht.Add("DueAmount", amounts[a]);
                            ht.Add("DueAmtInWords", clsnew.rupees(Convert.ToDecimal(amounts[a])));
                            ht.Add("TotalAmount", amounts[a]);
                            ht.Add("TotalAmtInWords", clsnew.rupees(Convert.ToDecimal(amounts[a])));
                            ht.Add("SaleID", Convert.ToInt32(Session["SaleID"].ToString()));

                            ht.Add("FlatID", Convert.ToInt32(Session["FlatID"].ToString()));
                            if (Session["TowerID"] != null)
                            {
                                ht.Add("TowerID", Convert.ToInt32(Session["TowerID"].ToString()));
                            }
                            ht.Add("saleprice", saleprice);
                            ht.Add("InstallmentOrder", Eventorder);

                            if (InstallmentNo == "Booking Amount" || InstallmentNo == "Possession Amount")
                            {
                                ht.Add("Type", 0);
                                ht.Add("EventID", 0);
                                ht.Add("EventName", InstallmentNo);
                                ht.Add("InstallmentNo", InstallmentNo);
                            }
                            else
                            {
                                if (!String.IsNullOrWhiteSpace(EventIdd[j]))
                                { ht.Add("EventID", Convert.ToInt32(EventIdd[j])); }
                                ht.Add("EventName", EvNames[j]);
                                ht.Add("InstallmentNo", EvNames[j]);
                                j++;

                                ht.Add("Type", 1);
                            }
                            ht.Add("Activity", "ADD");
                            if (bspPer[a] != null && !String.IsNullOrWhiteSpace(bspPer[a]))
                            {
                                ht.Add("Bsp", bspPer[a]);
                            }
                            if (obj.ExecuteProcedure("InsertInstallmentDetailCombo", ht))
                            {
                            }
                        }
                    }
                }
                #endregion
            }
        }
        public string GetInstallments(string saledate, string saleprice, int ddlInstallment, int ddlInterval, int PlanType, string plcPrice, string saleID, string flatid, string PlanTypeID)
        {
            if (saleID != "")
            {
                Session["SaleID"] = saleID;
            }
            if (flatid != "")
            {
                Session["FlatId"] = flatid;
            }
            // Session["SaleID"] = "3948";
            Session["PlanType"] = PlanType;
            StringBuilder l_StringBuilder = new StringBuilder();
            if (PlanType == 1)
            {
                #region Down Payment

                Session["table"] = null;
                NormalDownPaymentPlan(saledate, saleprice, ddlInstallment, ddlInstallment, PlanType, plcPrice);

                DataTable tb = new DataTable();
                if (Session["table"] != null)
                {
                    tb = Session["table"] as DataTable;
                    if (tb.Rows.Count > 0)
                    {
                        l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                        l_StringBuilder.Append("<thead><tr><th>Installment No.</th><th>Due Date</th><th>Amount</th></tr></thead><tbody>");

                        double total = 0;
                        int k = 0;
                        for (int j = 0; j < tb.Rows.Count; j++)
                        {

                            l_StringBuilder.Append("<tr><td>" + tb.Rows[j]["InstallmentNo"] + "</td><td>" + tb.Rows[j]["DueDate"] + "</td></td><td class='dueamount' id='bspdueamount" + k + "'>" + tb.Rows[j]["DueAmount"] + "</td><td id='bspbaseamount" + k + "' style='display:none'>" + tb.Rows[j]["DueAmount"] + "</td><tr>");
                            total += double.Parse(Convert.ToString(tb.Rows[j]["DueAmount"]));
                        }
                        l_StringBuilder.Append("<td colspan='2'>Total</td><td><b>" + total.ToString() + "</b></td></tr></table>");

                    }
                }


                #endregion Normal
            }
            else if (PlanType == 2)
            {
                Session["table"] = null;
                #region Construction plan
                ConstructionPlan(saledate, saleprice, ddlInstallment, ddlInterval, PlanType, plcPrice, PlanTypeID);
                ConstructionEvent();
                DataTable tb = new DataTable();
                if (Session["table"] != null)
                {
                    tb = Session["table"] as DataTable;
                    if (tb.Rows.Count > 0)
                    {
                        l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                        l_StringBuilder.Append("<thead><tr><th>Installment No.</th><th>Event</th><th>Due Date</th><th style='width:60px'>BSP %</th><th style='width:105px'>PLC % (If Any)</th><th>Amount</th></tr></thead><tbody>");

                        DateTime DueDate;
                        string SDate = "";
                        double total = 0;
                        int k = 0;
                        /*Balkrishna */
                        DueDate = Convert.ToDateTime(obj.Text_IndianDateFormat(Convert.ToString(saledate)));
                        for (int j = 0; j < tb.Rows.Count; j++)
                        {
                            k++;
                            if (j == 0)
                            {
                                tb.Rows[j]["DueDate"] = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                            }
                            else if (j == 1)
                            {                                                                          /* Add By Balkrishna */
                                DueDate = Convert.ToDateTime(DueDate.AddDays(60).ToString());
                                SDate = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                                tb.Rows[j]["DueDate"] = SDate;
                            }
                            else
                            {
                                DueDate = Convert.ToDateTime(DueDate.AddMonths(4).ToString());                 /*Balkrishna */
                                SDate = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                                tb.Rows[j]["DueDate"] = SDate;
                            }
                            if (k >= 4)
                            {
                                tb.Rows[j]["DueDate"] = "";
                            }
                            l_StringBuilder.Append("<tr><td>" + tb.Rows[j]["InstallmentNo"] + "</td><td>" + GetEventDetails(Convert.ToString(tb.Rows[j]["InstallmentNo"])) + "</td><td><input type='text' name='txtDueDate' value='" + tb.Rows[j]["DueDate"] + "' ></td><td  style='width:60px'><input id='" + k + "_" + tb.Rows[j]["BSP"] + "' type='text'  style='width:60px' class='bsppercentage bsppercentage_" + k + "' name='txtBsp' onblur='calculatebsp(this.value,this.id)' value='" + tb.Rows[j]["BSP"] + "' ></td><td><input id='" + k + "' type='text'  style='width:60px' class='plcpercentage' name='txtplc' onblur='calculatePLC(this.value,this.id)' value='" + tb.Rows[j]["PLC"] + "' ></td><td class='dueamount' id='bspdueamount" + k + "'>" + tb.Rows[j]["DueAmount"] + "</td><td id='bspbaseamount" + k + "' style='display:none'>" + tb.Rows[j]["DueAmount"] + "</td><tr>");
                            total += double.Parse(Convert.ToString(tb.Rows[j]["DueAmount"]));
                        }
                        l_StringBuilder.Append("<td colspan='5'>Total</td><td><b>" + total.ToString() + "</b></td></tr></table>");

                    }
                }
                #endregion construction plan
            }
            else if (PlanType == 3)
            {
                #region NormalPlan
                Hashtable ht = new Hashtable();
                ht.Add("FlatId", Session["FlatId"]);
                DataTable dt = new DataTable();
                dt.Columns.Add("DueDate", typeof(string));
                dt.Columns.Add("Event", typeof(string));
                dt.Columns.Add("AmountPer", typeof(decimal));
                dt.Columns.Add("Amount", typeof(decimal));

                l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                //l_StringBuilder.Append("<thead><tr><th>Due Date</th><th>Event</th><th>%</th><th>Amount</th></tr></thead><tbody>");
                l_StringBuilder.Append("<thead><tr><th>Installment No.</th><th>Due Date</th><th style='width:60px'> BSP %</th><th style='width:105px'> PLC % (If Any)</th><th>Amount</th></tr></thead><tbody>");
                double total = Convert.ToDouble(saleprice);
                double Gtotal = 0;
                decimal bookingAmount = 500000;
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.DateSeparator = "/";
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                DateTime DueDate = Convert.ToDateTime(saledate, dtinfo);
                for (int i = 0; i < ddlInstallment; i++)
                {
                    DataRow row = dt.NewRow();
                    row["DueDate"] = DueDate.AddMonths(i * ddlInterval).ToString("dd/MM/yyyy");
                    string InstallmentNo = "";
                    if (i == 0)
                    {
                        row["Event"] = "Booking Amount"; InstallmentNo = "Booking Amount";
                    }
                    else if (i == ddlInstallment - 1)
                    {
                        row["Event"] = "Possession Amount"; InstallmentNo = "Possession Amount";
                    }
                    else { row["Event"] = i; InstallmentNo = i.ToString(); }

                    if (i == 0)
                        row["Amount"] = bookingAmount;
                    else
                    {
                        row["Amount"] = (Convert.ToDecimal(saleprice) - bookingAmount) / (ddlInstallment - 1);
                    }
                    if (ddlInstallment == 1) // DevideByZero
                        row["AmountPer"] = 100;
                    else if (i == 0)
                        row["AmountPer"] = (bookingAmount * 100) / Convert.ToDecimal(saleprice);
                    else
                        row["AmountPer"] = (100 - ((bookingAmount * 100) / Convert.ToDecimal(saleprice))) / (ddlInstallment - 1);
                    // row["AmountPer"] = Convert.ToDecimal(saleprice) / ((Convert.ToDecimal(saleprice) - bookingAmount) / (ddlInstallment - 1));
                    dt.Rows.Add(row);
                    l_StringBuilder.Append("<tr><td>" + InstallmentNo + "</td><td><input type='text'  name='txtDueDate' value='" + row["DueDate"] + "' ></td><td  style='width:60px'><input id='" + i + "_" + row["AmountPer"] + "'  type='text'  style='width:60px' class='bsppercentage  bsppercentage_" + i + "' name='txtBsp' onblur='calculatebsp(this.value,this.id)' value='" + row["AmountPer"] + "' ></td><td><input id='" + i + "' type='text'  readonly = 'readonly'  style='width:105px' class='plcpercentage' onblur='calculatePLC(this.value,this.id)'  name='txtxPLC'  value='' ></td><td class='dueamount' id='bspdueamount" + i + "'>" + row["Amount"] + "</td><td id='bspbaseamount" + i + "' style='display:none'>" + row["Amount"] + "</td><tr>");

                    Gtotal = Gtotal + double.Parse(Convert.ToString(row["Amount"]));
                }
                Session["table"] = dt;
                l_StringBuilder.Append("<td colspan='5'>Total</td><td><b>" + total.ToString() + "</b></td></tr></tbody>");

                #endregion
            }
            else if (PlanType == 4 || PlanType == 5)
            {
                Session["table"] = null;
                #region Combo Plan
                ComboPlan(saledate, saleprice, ddlInstallment, ddlInterval, PlanType, plcPrice);
                //  ComboEvent();
                DataTable tb = new DataTable();
                if (Session["table"] != null)
                {
                    tb = Session["table"] as DataTable;
                    if (tb.Rows.Count > 0)
                    {
                        l_StringBuilder.Append("<table class='table table-bordered table-striped particular_tbl'>");
                        l_StringBuilder.Append("<thead><tr><th>Installment No.</th><th>Event</th><th>Due Date</th><th style='width:60px'> BSP %</th><th style='width:105px'> PLC % (If Any)</th><th>Amount</th></tr></thead><tbody>");

                        double total = 0;
                        int k = 0;
                        for (int j = 0; j < tb.Rows.Count; j++)
                        {
                            k++;
                            if (k >= 2)
                            {
                                tb.Rows[j]["DueDate"] = "";
                            }
                            if (j >= 0 && j < 3)
                            {
                                l_StringBuilder.Append("<tr><td>" + tb.Rows[j]["InstallmentID"] + "</td><td>" + GetComboEvents(Convert.ToString(tb.Rows[j]["InstallmentNo"]), Convert.ToString(tb.Rows[j]["InstallmentID"])) + "</td><td><input type='text'  name='txtDueDate' value='" + tb.Rows[j]["DueDate"] + "' ></td><td  style='width:60px'><input id='" + k + "_" + tb.Rows[j]["BSP"] + "'  type='text'  style='width:60px' class='bsppercentage  bsppercentage_" + k + "' name='txtBsp' onblur='calculatebsp(this.value,this.id)' value='" + tb.Rows[j]["BSP"] + "' ></td><td><input id='" + k + "' type='text'  readonly = 'readonly'  style='width:105px' class='plcpercentage' onblur='calculatePLC(this.value,this.id)' name='txtxPLC'  value='" + tb.Rows[j]["PLC"] + "' ></td><td class='dueamount' id='bspdueamount" + k + "'>" + tb.Rows[j]["DueAmount"] + "</td><td id='bspbaseamount" + k + "' style='display:none'>" + tb.Rows[j]["DueAmount"] + "</td><tr>");
                            }
                            else
                            {
                                l_StringBuilder.Append("<tr><td>" + tb.Rows[j]["InstallmentID"] + "</td><td>" + GetComboEvents(Convert.ToString(tb.Rows[j]["InstallmentNo"]), Convert.ToString(tb.Rows[j]["InstallmentID"])) + "</td><td><input type='text' name='txtDueDate' value='" + tb.Rows[j]["DueDate"] + "' ></td><td  style='width:60px'><input id='" + k + "_" + tb.Rows[j]["BSP"] + "' type='text'  style='width:60px' class='bsppercentage  bsppercentage_" + k + "' name='txtBsp' onblur='calculatebsp(this.value,this.id)' value='" + tb.Rows[j]["BSP"] + "' ></td><td><input id='" + k + "' type='text'  style='width:105px' class='plcpercentage' name='txtxPLC'  onblur='calculatePLC(this.value,this.id)' value='" + tb.Rows[j]["PLC"] + "' ></td><td class='dueamount' id='bspdueamount" + k + "'>" + tb.Rows[j]["DueAmount"] + "</td><td id='bspbaseamount" + k + "' style='display:none'>" + tb.Rows[j]["DueAmount"] + "</td><tr>");
                            }
                            total += double.Parse(Convert.ToString(tb.Rows[j]["DueAmount"]));

                        }
                        l_StringBuilder.Append("<td></td><td></td><td></td><td></td>Total<td></td><td><b id='bsptotal'>" + total.ToString() + "</b></td></tr></tbody></table>");

                    }
                }
                #endregion construction plan

            }
            return l_StringBuilder.ToString();
        }
        protected void ConstructionPlan(string saledate, string saleprice, int ddlInstallment, int ddlInterval, int PlanType, string plcPrice, string PlanTypeID)
        {
            Session["table"] = null;
            setTable();
            DateTime DueDate;
            decimal DueAmount;
            decimal TotalAmount;
            decimal BalanceAmount;
            decimal BookingAmt;
            decimal TotalPLC = !string.IsNullOrWhiteSpace(plcPrice) ? Convert.ToDecimal(plcPrice) : 0;
            //------------------
            Session["TableEvent"] = "";
            Hashtable ht = new Hashtable();
            if (Session["TowerID"] != null)
            {
                ht.Add("TowerID", Convert.ToInt32(Session["TowerID"].ToString()));
            }
            dtEvent = obj.GetDataTableFromProcedure("ViewEventByTowerID", ht);
            ddlInstallment = dtEvent.Rows.Count;
            if (dtEvent.Rows.Count > 0)
            {
                Session["TableEvent"] = dtEvent;
            }
            //-------------------
            DueDate = Convert.ToDateTime(obj.Text_IndianDateFormat(saledate));
            DueAmount = Math.Round(Convert.ToDecimal(saleprice), 0);

            TotalAmount = DueAmount;
            BalanceAmount = DueAmount;

            int j;
            int c = Convert.ToInt32(ddlInstallment);
            DataTable dtPayment = obj.GetDataTable("select SaleID from tblSPaymentDetail where RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"].ToString()));
            DataTable dt = new DataTable();
            dt = obj.GetDataTable("select InstallmentID,InstallmentNo,FlatID,SaleID,convert(varchar(50),DueDate,103) as DueDate,ceiling(round(DueAmount,0)) as DueAmount,DueAmtInWords,ServiceTaxAmount,ceiling(round(TotalAmount,0)) as TotalAmount,TotalAmtInWords,PayStatus,RecordStatus,EventID from tblSInstallmentDetail where RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"].ToString()) + " Order by InstallmentID asc");
            if (dt.Rows.Count == 0)
            {
                float cas2 = 20.00f, cas3 = 10.00f;
                decimal tamt = 0;
                BookingAmt = 500000;
                float cas1 = (float)BookingAmt * 100 / (float)TotalAmount;
                cas3 = (100 - cas1) / 9;
                for (int i = 1; i <= ddlInstallment; i++)
                {
                    switch (i)
                    {
                        case 1:
                            DueAmount = BookingAmt;
                            addRow("Booking Amount ", Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, cas1); tamt = tamt + DueAmount;
                            break;
                        case 2:
                            DueAmount = Convert.ToDecimal((float)TotalAmount * cas3 / 100f);
                            //DueAmount = DueAmount - BookingAmt;
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 3:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 4:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 5:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 6:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 7:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 8:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 9:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100));
                            addRow((i - 1).ToString(), Convert.ToInt32(Session["SaleID"]), DueAmount, cas3); tamt = tamt + DueAmount;
                            break;
                        case 10:
                            DueAmount = Convert.ToDecimal(((float)TotalAmount * cas3 / 100)); tamt = tamt + DueAmount;
                            decimal sp = Math.Round(Convert.ToDecimal(saleprice), 0);
                            if (sp < tamt) { DueAmount = DueAmount - (tamt - sp); }
                            addRow("Possession Amount", Convert.ToInt32(Session["SaleID"]), DueAmount, cas3);
                            break;
                    }
                    c = c + 1;
                }
            }
            else
            {
                Session["table"] = dt;
            }
        }
        protected void ComboPlan(string saledate, string saleprice, int ddlInstallment, int ddlInterval, int PlanType, string plcTotal)
        {
            Session["table"] = null;
            setTable();
            DateTime DueDate;
            decimal DueAmount;
            decimal TotalAmount;
            decimal BalanceAmount;
            decimal BookingAmt;
            decimal PossessionAmt;
            decimal TotalPLC = !String.IsNullOrWhiteSpace(plcTotal) ? Convert.ToDecimal(plcTotal) : 0;
            Session["TableEvent"] = "";
            DueDate = Convert.ToDateTime(obj.Text_IndianDateFormat(saledate));
            DueAmount = Math.Round(Convert.ToDecimal(saleprice), 0);
            TotalAmount = DueAmount;
            BalanceAmount = DueAmount;
            int j;
            DataTable dtPayment = obj.GetDataTable("select SaleID from tblSPaymentDetail where RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"].ToString()));
            DataTable dt = new DataTable();
            dt = obj.GetDataTable("select InstallmentID,InstallmentNo,FlatID,SaleID,convert(varchar(50),DueDate,103) as DueDate,ceiling(round(DueAmount,0)) as DueAmount,DueAmtInWords,ServiceTaxAmount,ceiling(round(TotalAmount,0)) as TotalAmount,TotalAmtInWords,PayStatus,RecordStatus,EventID from tblSInstallmentDetail where RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"].ToString()) + " Order by InstallmentID asc");
            Int32 IsEdit = 0;
            Hashtable ht1 = new Hashtable();
            ht1.Add("FlatId", Convert.ToInt32(Session["FlatId"].ToString()));
            dtEvent = obj.GetDataTableFromProcedure("GetEventsByBlockComboPlan", ht1);
            if (dtEvent.Rows.Count > 0)
            {
                Session["TableEvent"] = dtEvent;
            }
            if (dtEvent.Rows.Count > 0)
            {
                float cas1 = 0.00f, cas2 = 20.00f, cas3 = 10.00f;
                decimal tamt = 0;
                if (PlanType == 5)
                {
                    cas1 = 50;
                    cas2 = 50f / 9f;
                    cas3 = cas2;
                    BookingAmt = TotalAmount / 2;
                }
                else
                {
                    cas1 = 500000 * 100 / (float)TotalAmount;
                    cas2 = (100 - cas1) / 9;
                    cas3 = cas2;
                    BookingAmt = 500000;
                }
                addRowCombo("Booking Amount", "Booking Amount", Convert.ToInt32(Session["SaleID"]), BookingAmt, DueDate, "0", cas1, 0);
                for (Int32 i = 0; i < dtEvent.Rows.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas2 / 100)), 2);
                            // DueAmount = DueAmount - BookingAmt;
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas2);
                            break;
                        case 1:
                            // DueAmount = Math.Round((TotalAmount * Convert.ToInt32(dtEvent.Rows[i]["AmtPer"]) / 100), 0) - Math.Round((TotalAmount * 20 / 100), 0);
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                        case 2:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                        case 3:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                        case 4:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            if (TotalPLC > 0)
                            {
                                DueAmount = DueAmount + TotalPLC;
                                addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3, 100);
                            }
                            else
                            {
                                addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            }
                            break;
                        case 5:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                        case 6:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                        case 7:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                        case 8:
                            DueAmount = Math.Round((Convert.ToDecimal((float)TotalAmount * cas3 / 100)), 2);
                            addRowCombo(Convert.ToString(dtEvent.Rows[i]["EventOrder"]), Convert.ToString(dtEvent.Rows[i]["EventName"]), Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate, Convert.ToString(dtEvent.Rows[i]["EventOrder"]), cas3);
                            break;
                    }
                }
            }
        }
        protected void NormalDownPaymentPlan(string saledate, string saleprice, int ddlInstallment, int ddlInterval, int PlanType, string plcTotal)
        {
            Session["table"] = null;
            setTable();
            DateTime DueDate;
            decimal DueAmount;
            decimal TotalAmount;
            decimal BalanceAmount;
            decimal BookingAmt;
            decimal PossessionAmt;
            decimal TotalPLC = !String.IsNullOrWhiteSpace(plcTotal) ? Convert.ToDecimal(plcTotal) : 0;
            Session["TableEvent"] = "";
            DueDate = Convert.ToDateTime(obj.Text_IndianDateFormat(saledate));
            DueAmount = Math.Round(Convert.ToDecimal(saleprice), 0);
            TotalAmount = DueAmount;
            BalanceAmount = DueAmount;
            int j;
            BookingAmt = 500000;
            DataTable dtPayment = obj.GetDataTable("select SaleID from tblSPaymentDetail where RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"].ToString()));
            DataTable dt = new DataTable();
            dt = obj.GetDataTable("select InstallmentID,InstallmentNo,FlatID,SaleID,convert(varchar(50),DueDate,103) as DueDate,ceiling(round(DueAmount,0)) as DueAmount,DueAmtInWords,ServiceTaxAmount,ceiling(round(TotalAmount,0)) as TotalAmount,TotalAmtInWords,PayStatus,RecordStatus,EventID from tblSInstallmentDetail where RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"].ToString()) + " Order by InstallmentID asc");
            addRowNormalDown("Booking Amount", "Booking Amount", Convert.ToInt32(Session["SaleID"]), BookingAmt, DueDate, 1.ToString());
            DueAmount = Math.Round(Math.Round((TotalAmount * 20 / 100), 2) - BookingAmt);
            addRowNormalDown("1", "At the time of Allotement", Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate.AddDays(15).Date, 2.ToString());
            DueAmount = Math.Round(Math.Round((TotalAmount * 90 / 100), 2) - Math.Round((TotalAmount * 20 / 100), 2));
            if (TotalPLC > 0)
            {
                DueAmount = Math.Round(DueAmount + Convert.ToDecimal(TotalPLC));
            }
            addRowNormalDown("2", "Within 60 days of booking (PLC Added If Any) ", Convert.ToInt32(Session["SaleID"]), DueAmount, DueDate.AddDays(60).Date, 3.ToString());
            DueAmount = Math.Round((TotalAmount * 10 / 100), 0);

            addRowNormalDown("3", "At the time of Possession", Convert.ToInt32(Session["SaleID"]), DueAmount, new DateTime().Date, 4.ToString());
        }

        private void addRowNormalDown(string InsId, string insNo, Int32 SaleID, decimal DueAmount, DateTime DueDate, string EventOrder, int BSPAmount = 0, Int64 PLCAmount = 0)
        {
            try
            {
                DataTable tb = Session["table"] as DataTable;
                DataRow dr = tb.NewRow();
                dr["InstallmentID"] = InsId;
                dr["InstallmentNo"] = insNo;
                dr["SaleID"] = SaleID;
                if (new DateTime().Date != DueDate)
                {
                    dr["DueDate"] = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                }
                dr["DueAmount"] = Math.Round(DueAmount, 0);
                dr["TotalAmount"] = Math.Round(DueAmount, 0);
                dr["PayStatus"] = 0;
                dr["EventID"] = Convert.ToInt32(EventOrder);

                if (BSPAmount != 0)
                {
                    dr["BSP"] = BSPAmount;
                }

                if (PLCAmount != 0)
                {
                    dr["PLC"] = PLCAmount;
                }

                tb.Rows.Add(dr);
                Session["table"] = tb;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //  Page.RegisterClientScriptBlock("item row", "<script>alert('" + msg + "');</script>");
            }
        }
        private void addRowCombo(string InsId, string insNo, Int32 SaleID, decimal DueAmount, DateTime DueDate, string EventOrder, float BSPAmount = 0, Int64 PLCAmount = 0)
        {
            try
            {
                DataTable tb = Session["table"] as DataTable;
                DataRow dr = tb.NewRow();
                dr["InstallmentID"] = InsId;
                dr["InstallmentNo"] = insNo;
                dr["SaleID"] = SaleID;
                dr["DueDate"] = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                dr["DueAmount"] = Math.Round(DueAmount, 2);
                dr["TotalAmount"] = Math.Round(DueAmount, 2);
                dr["PayStatus"] = 0;
                dr["EventID"] = Convert.ToInt32(EventOrder);

                if (BSPAmount != 0)
                {
                    //dr["BSP"] =Math.Round(Convert.ToDecimal(BSPAmount),2);
                    dr["BSP"] = BSPAmount;
                }

                if (PLCAmount != 0)
                {
                    dr["PLC"] = PLCAmount;
                }

                tb.Rows.Add(dr);
                Session["table"] = tb;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //  Page.RegisterClientScriptBlock("item row", "<script>alert('" + msg + "');</script>");
            }
        }
        private void addRow(string insNo, Int32 SaleID, decimal DueAmount, float BSP = 0, decimal PLC = 0)
        {
            try
            {
                DataTable tb = Session["table"] as DataTable;
                DataRow dr = tb.NewRow();
                dr["InstallmentID"] = 0;
                dr["InstallmentNo"] = insNo;
                dr["SaleID"] = SaleID;
                //dr["DueDate"] = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                dr["DueAmount"] = Math.Round(DueAmount, 2);
                dr["TotalAmount"] = Math.Round(DueAmount, 2);
                dr["PayStatus"] = 0;
                dr["EventID"] = 0;
                if (BSP > 0)
                {
                    dr["BSP"] = BSP;
                }
                if (PLC > 0)
                {
                    dr["PLC"] = PLC;
                }
                tb.Rows.Add(dr);
                Session["table"] = tb;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //Page.RegisterClientScriptBlock("item row", "<script>alert('" + msg + "');</script>");
            }
        }
        private void addRow(string insNo, Int32 SaleID, decimal DueAmount, DateTime DueDate, float BSP = 0)
        {
            try
            {
                DataTable tb = Session["table"] as DataTable;
                DataRow dr = tb.NewRow();
                dr["InstallmentID"] = 0;
                dr["InstallmentNo"] = insNo;
                dr["SaleID"] = SaleID;
                dr["DueDate"] = obj.DB_IndianDateFormat(DueDate.ToShortDateString());
                dr["DueAmount"] = Math.Round(DueAmount, 2);
                dr["TotalAmount"] = Math.Round(DueAmount, 2);
                dr["PayStatus"] = 0;
                dr["EventID"] = 0;
                if (BSP > 0)
                {
                    dr["BSP"] = BSP;
                }

                tb.Rows.Add(dr);
                Session["table"] = tb;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //  Page.RegisterClientScriptBlock("item row", "<script>alert('" + msg + "');</script>");
            }
        }
        public void ComboEvent()
        {
            Session["TableEvent"] = "";
            Hashtable ht = new Hashtable();
            ht.Add("FlatId", Convert.ToInt32(Session["FlatId"].ToString()));
            dtEvent = obj.GetDataTableFromProcedure("GetEventsByBlockComboPlan", ht);
            if (dtEvent.Rows.Count > 0)
            {
                Session["TableEvent"] = dtEvent;
            }

        }
        public void ConstructionEvent()
        {
            Session["TableEvent"] = "";
            Hashtable ht = new Hashtable();
            if (Session["TowerID"] != null)
            {
                ht.Add("TowerID", Convert.ToInt32(Session["TowerID"].ToString()));
            }
            dtEvent = obj.GetDataTableFromProcedure("ViewEventByTowerID", ht);
            if (dtEvent.Rows.Count > 0)
            {
                Session["TableEvent"] = dtEvent;
            }

        }
        private StringBuilder GetComboEvents(string Event, string InstallmentNumber)
        {
            StringBuilder ddl = new StringBuilder();

            if (InstallmentNumber == "Booking Amount")
            {
                ddl.Append("Booking Amount");
                return ddl;
            }
            DataTable dt = new DataTable();
            if (Session["TableEvent"] != null)
            {
                ddl.Append("<select id='Events' class='dropdwn'>");
                dt = (DataTable)Session["TableEvent"];
                for (Int32 i = 0; i < dt.Rows.Count; i++)
                {

                    string EventName = Convert.ToString(dt.Rows[i]["EventName"]);
                    if (EventName != Event)
                        ddl.Append("<option value='" + Convert.ToString(dt.Rows[i]["EventId"]) + "'>" + EventName + "</option>");
                    else
                        ddl.Append("<option value='" + Convert.ToString(dt.Rows[i]["EventId"]) + "'>" + EventName + "</option>");
                }
                ddl.Append("</select>");
            }
            return ddl;
        }
        private StringBuilder GetEventDetails(string Event)
        {
            StringBuilder ddl = new StringBuilder();
            if (Event.Length > 5)
            {
                ddl.Append(Event);
                return ddl;
            }
            DataTable dt = new DataTable();
            if (Session["TableEvent"] != null)
            {
                ddl.Append("<select id='Events' class='dropdwn'>");
                dt = (DataTable)Session["TableEvent"];
                for (Int32 i = 0; i < dt.Rows.Count; i++)
                {
                    string EventName = Convert.ToString(dt.Rows[i]["EventName"]);
                    if (EventName != Event)
                        ddl.Append("<option value='" + Convert.ToString(dt.Rows[i]["EventId"]) + "'>" + EventName + "</option>");
                    else
                        ddl.Append("<option value='" + Convert.ToString(dt.Rows[i]["EventId"]) + "'>" + Event + "</option>");
                }
                ddl.Append("</select>");
            }
            return ddl;
        }

        public JsonResult GetTower(int pID)
        {
            return Json(SbpEntity.GetBlockByPID(pID).ToList());
        }
        public JsonResult GetFlatByBId(string bId)
        {
            var FlatNameList = SbpEntity.Get_FlatIdByTowerBId(bId).ToList();
            return Json(FlatNameList);
        }
        // Save New Customer data (Booking or sale)
        public HttpResponseMessage SaveCustomer(FlatSale newCust)
        {
            int ii = newCust.PlanID.Value;
            try
            {
                if (newCust.AppTitle == null) newCust.AppTitle = "";
                if (newCust.AppFName == null) newCust.AppFName = "";
                if (newCust.AppMName == null) newCust.AppMName = "";
                if (newCust.AppLName == null) newCust.AppLName = "";
                if (newCust.CoTitle == null) newCust.CoTitle = "";
                if (newCust.CoFName == null) newCust.CoFName = "";
                if (newCust.CoMName == null) newCust.CoMName = "";
                if (newCust.CoLName == null) newCust.CoLName = "";
                DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
                dtinfo.DateSeparator = "/";
                dtinfo.ShortDatePattern = "dd/MM/yyyy";
                newCust.BookingDate = Convert.ToDateTime(newCust.SrtSaleDate, dtinfo);
                //newCust.BookingDate = newCust.SaleDate;
                int i = SbpEntity.INSERT_RBookFlat(newCust.FlatName, newCust.SaleDate, newCust.BookingAmount, newCust.AppFName, newCust.AppMName, newCust.AppLName, newCust.Title,
                newCust.PName, newCust.Address1, newCust.Address2, newCust.City, newCust.Distt, newCust.State, newCust.Country, newCust.AppPAN, newCust.MobileNo, newCust.DateOfBirth,
                newCust.CoFName, newCust.CoMName, newCust.CoLName, newCust.CoAddress1,
                newCust.CoAddress2, newCust.CoCity, newCust.CoState, newCust.CoCountry, newCust.CoPAN, newCust.CoMobileNo, newCust.AlternateMobile, newCust.LandLine,
                newCust.EmailID, newCust.AlternateEmail, newCust.LoanAmount, newCust.LienField, newCust.BankID, newCust.IsPAN,
                newCust.IsPhoto, newCust.IsAddressPf, newCust.Type, newCust.CustomerID, newCust.SecCoFName, newCust.SecCoMName, newCust.SecCoLName, newCust.SecCoAddress1,
                newCust.SecCoAddress2, newCust.SecCoCity, newCust.SecCoState, newCust.SecCoCountry, newCust.SecCoMobileNo, newCust.SecCoPAN,
                newCust.CoTitle, newCust.CoPName, newCust.SecCoTitle, newCust.SecCoPName, newCust.IsConstruction, newCust.AppTitle, newCust.CoAppTitle, newCust.SecCoAppTitle, newCust.IsRationCard, newCust.IsDrivingLicence, newCust.IsVoterCard, newCust.IsPassport, newCust.PhotoImagePath, newCust.AddPfImagePath, newCust.DVImagePath, newCust.VoterCardImagePath, newCust.PassportImagePath,
                newCust.RationCardImagePath, newCust.Remarks, newCust.CoDOB, newCust.SecCoDOB, newCust.PANImagePath, newCust.CoIsPAN, newCust.CoIsPhoto, newCust.CoIsAddressPf, newCust.CoIsRationCard, newCust.CoIsDrivingLicence, newCust.CoIsVoterCard, newCust.CoIsPassport, newCust.CoPhotoImagePath, newCust.CoAddPfImagePath, newCust.CoDVImagePath, newCust.CoVoterCardImagePath, newCust.CoPhotoImagePath,
                newCust.CoRationCardImagePath, newCust.CoPANImagePath, newCust.SecCoIsPAN, newCust.SecCoIsPhoto, newCust.CoIsAddressPf, newCust.SecCoIsRationCard, newCust.SecCoIsDrivingLicence, newCust.SecCoIsVoterCard, newCust.SecCoIsPassport, newCust.SecCoPhotoImagePath,
                newCust.SecCoAddPfImagePath, newCust.SecCoDVImagePath, newCust.SecCoVoterCardImagePath, newCust.SecCoPhotoImagePath, newCust.SecCoRationCardImagePath, newCust.SecCoPhotoImagePath, newCust.BankBranch, newCust.PinCode, newCust.ExecutiveName, newCust.CoPinCode, newCust.SecCoPinCode, newCust.TransferAmount, newCust.affidavit, newCust.PropertyID, newCust.PaymentFor, newCust.BookingDate, newCust.PropertyTypeID, newCust.PropertySizeID, newCust.PlanID, User.Identity.Name, newCust.SaleRate, newCust.SaleRateInWords);
                DataTable dt = obj.GetDataTable("select FlatID from tblsFlat where FlatName='" + newCust.FlatName + "' and PID='" + newCust.PropertyID + "' and Status=1");
                if (dt.Rows.Count > 0)
                {
                    Session["FlatId"] = dt.Rows[0]["FlatID"].ToString();
                }
                else
                {
                    Session["FlatId"] = newCust.FlatID;
                }
                Session["PID"] = newCust.PropertyID;
                Session["FlatName"] = newCust.FlatName;
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public ActionResult EditCustomer(string Id)
        {
            ViewBag.ID = Id;
            return View();
        }

        public string custDetail(string saleId)
        {
            Customer custDetail = new Customer();
            DataTable dtCust;
            dtCust = obj.GetDataTable("select t1.* from Customer t1 left outer join tblSSaleFlat t2 on t2.customerId=t1.customerid where t2.saleid=" + saleId);
            if (dtCust.Rows.Count > 0)
            {
                custDetail.CustomerID = Convert.ToInt32(dtCust.Rows[0]["CustomerID"]);
                custDetail.AppTitle = Convert.ToString(dtCust.Rows[0]["AppTitle"]);
                custDetail.FName = Convert.ToString(dtCust.Rows[0]["FName"]);
                custDetail.MName = Convert.ToString(dtCust.Rows[0]["MName"]);
                custDetail.LName = Convert.ToString(dtCust.Rows[0]["LName"]);
                custDetail.PName = Convert.ToString(dtCust.Rows[0]["PName"]);
                custDetail.Address1 = Convert.ToString(dtCust.Rows[0]["Address1"]);
                custDetail.Address2 = Convert.ToString(dtCust.Rows[0]["Address2"]);
                custDetail.City = Convert.ToString(dtCust.Rows[0]["City"]);
                custDetail.State = Convert.ToString(dtCust.Rows[0]["State"]);
                custDetail.Country = Convert.ToString(dtCust.Rows[0]["Country"]);
                custDetail.MobileNo = Convert.ToString(dtCust.Rows[0]["MobileNo"]);
                custDetail.PAN = Convert.ToString(dtCust.Rows[0]["PAN"]);
                custDetail.AlternateMobile = Convert.ToString(dtCust.Rows[0]["AlternateMobile"]);
                string dob = dtCust.Rows[0]["DateOfBirth"].ToString();
                if (dob == null || dob == "") custDetail.DateOfBirthst = "";
                else
                    custDetail.DateOfBirthst = Convert.ToDateTime(dtCust.Rows[0]["DateOfBirth"]).ToString("dd/MM/yyyy");
                custDetail.EmailID = Convert.ToString(dtCust.Rows[0]["EmailID"]);
                custDetail.LandLine = Convert.ToString(dtCust.Rows[0]["LandLine"]);
                custDetail.AlternateEmail = Convert.ToString(dtCust.Rows[0]["AlternateEmail"]);
                custDetail.CoAppTitle = Convert.ToString(dtCust.Rows[0]["CoAppTitle"]);
                custDetail.CoFName = Convert.ToString(dtCust.Rows[0]["CoFName"]);
                custDetail.CoMName = Convert.ToString(dtCust.Rows[0]["CoMName"]);
                custDetail.CoLName = Convert.ToString(dtCust.Rows[0]["CoLName"]);
                custDetail.CoTitle = Convert.ToString(dtCust.Rows[0]["CoTitle"]);
                custDetail.CoPName = Convert.ToString(dtCust.Rows[0]["CoPName"]);
                custDetail.CoMobileNo = Convert.ToString(dtCust.Rows[0]["CoMobileNo"]);
                custDetail.CoPAN = Convert.ToString(dtCust.Rows[0]["CoPAN"]);
                custDetail.CoAddress1 = Convert.ToString(dtCust.Rows[0]["CoAddress1"]);
                custDetail.CoAddress2 = Convert.ToString(dtCust.Rows[0]["CoAddress2"]);
                custDetail.CoCity = Convert.ToString(dtCust.Rows[0]["CoCity"]);
                custDetail.CoState = Convert.ToString(dtCust.Rows[0]["CoState"]);
                custDetail.CoCountry = Convert.ToString(dtCust.Rows[0]["CoCountry"]);
                custDetail.CoPinCode = Convert.ToString(dtCust.Rows[0]["CoPinCode"]);

                if (dtCust.Rows[0]["CoDOB"].ToString() == null || dtCust.Rows[0]["CoDOB"].ToString() == "")
                    custDetail.CoDOBst = "";
                else
                    custDetail.CoDOBst = Convert.ToDateTime(dtCust.Rows[0]["CoDOB"]).ToString("dd/MM/yyyy");
                custDetail.SecCoAppTitle = Convert.ToString(dtCust.Rows[0]["SecCoAppTitle"]);
                custDetail.SecCoPName = Convert.ToString(dtCust.Rows[0]["SecCoPName"]);
                custDetail.SecCoFName = Convert.ToString(dtCust.Rows[0]["SecCoFName"]);
                custDetail.SecCoMName = Convert.ToString(dtCust.Rows[0]["SecCoMName"]);
                custDetail.SecCoLName = Convert.ToString(dtCust.Rows[0]["SecCoLName"]);
                custDetail.SecCoAddress1 = Convert.ToString(dtCust.Rows[0]["SecCoAddress1"]);
                custDetail.SecCoAddress2 = Convert.ToString(dtCust.Rows[0]["SecCoAddress2"]);
                custDetail.SecCoMobileNo = Convert.ToString(dtCust.Rows[0]["SecCoMobileNo"]);
                custDetail.SecCoPAN = Convert.ToString(dtCust.Rows[0]["SecCoPAN"]);
                custDetail.SecCoCity = Convert.ToString(dtCust.Rows[0]["SecCoCity"]);
                custDetail.SecCoState = Convert.ToString(dtCust.Rows[0]["SecCoState"]);
                custDetail.SecCoCountry = Convert.ToString(dtCust.Rows[0]["SecCoCountry"]);
                custDetail.SecCoPinCode = Convert.ToString(dtCust.Rows[0]["SecCoPinCode"]);
                string sodob = dtCust.Rows[0]["SecCoDOB"].ToString();
                if (sodob == null || sodob == "")
                    custDetail.SecCoDOBst = "";
                else
                    custDetail.SecCoDOBst = Convert.ToDateTime(dtCust.Rows[0]["SecCoDOB"]).ToString("dd/MM/yyyy");
                custDetail.ExecutiveName = Convert.ToString(dtCust.Rows[0]["ExecutiveName"]);
                if (dtCust.Rows[0]["LoanAmount"].ToString() == null || dtCust.Rows[0]["LoanAmount"].ToString() == "")
                    custDetail.LoanAmount = 0;
                else
                    custDetail.LoanAmount = Convert.ToDecimal(dtCust.Rows[0]["LoanAmount"]);
                custDetail.BankBranch = Convert.ToString(dtCust.Rows[0]["BankBranch"]);
                if (dtCust.Rows[0]["BankID"].ToString() == null || dtCust.Rows[0]["BankID"].ToString() == "")
                    custDetail.BankID = 0;
                else
                    custDetail.BankID = Convert.ToInt32(dtCust.Rows[0]["BankID"]);
                custDetail.BankAddress = Convert.ToString(dtCust.Rows[0]["BankAddress"]);
                custDetail.Remarks = Convert.ToString(dtCust.Rows[0]["Remarks"]);
                custDetail.LienField = Convert.ToString(dtCust.Rows[0]["LienField"]);

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(custDetail);
        }

        public string updateCustomer(Customer cDetail)
        {
            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            if (cDetail.DateOfBirthst != null)
            {
                cDetail.DateOfBirth = Convert.ToDateTime(cDetail.DateOfBirthst, dtinfo);
            }
            if (cDetail.CoDOBst != null)
            {
                cDetail.CoDOB = Convert.ToDateTime(cDetail.CoDOBst, dtinfo);

            }
            if (cDetail.SecCoDOBst != null)
            {
                cDetail.SecCoDOB = Convert.ToDateTime(cDetail.SecCoDOBst, dtinfo);
            }

            int i = SbpEntity.sp_Customer(cDetail.CustomerID, cDetail.AppTitle, cDetail.FName, cDetail.MName, cDetail.LName, cDetail.Title, cDetail.PName, cDetail.Address1, cDetail.Address2, cDetail.City, cDetail.State, cDetail.Country, cDetail.PAN, cDetail.MobileNo, cDetail.DateOfBirth, cDetail.CoAppTitle, cDetail.CoFName, cDetail.CoMName, cDetail.CoLName, cDetail.CoAddress1, cDetail.CoAddress2, cDetail.CoCity, cDetail.CoState, cDetail.CoCountry, cDetail.CoPAN, cDetail.CoMobileNo, cDetail.AlternateMobile, cDetail.LandLine, cDetail.EmailID, cDetail.AlternateEmail, cDetail.LoanAmount, cDetail.LienField, cDetail.BankID, cDetail.BankBranch, cDetail.SecCoAppTitle, cDetail.SecCoFName, cDetail.SecCoMName, cDetail.SecCoLName, cDetail.SecCoAddress1, cDetail.SecCoAddress2, cDetail.SecCoCity, cDetail.SecCoState, cDetail.SecCoCountry, cDetail.SecCoMobileNo, cDetail.SecCoPAN, cDetail.CoTitle, cDetail.CoPName, cDetail.SecCoTitle, cDetail.SecCoPName, cDetail.Remarks, cDetail.CoDOB, cDetail.SecCoDOB, cDetail.PinCode, cDetail.ExecutiveName, cDetail.CoPinCode, cDetail.SecCoPinCode);
            return i.ToString();
        }

        private List<FlatDetail> GetFlatDetails(string SaleId)
        {
            List<FlatDetail> l_FlatDetail = new List<FlatDetail>();
            FlatDetail flatDetail = new FlatDetail();
            DataTable dtSale;
            dtSale = obj.GetDataTable("select d.towerid,a.*,b.FlatName,c.BlockName from tblSSaleFlat a,tblSFlat b,tblSBlocks c,tblSTower d where b.FlatID=a.FlatID and b.TowerID=d.TowerID and d.BID=c.BID and a.SaleID=" + SaleId);
            if (dtSale.Rows.Count > 0)
            {
                flatDetail.FlatID = dtSale.Rows[0]["FlatID"].ToString();
                flatDetail.FlatID = dtSale.Rows[0]["FlatID"].ToString();
                Session["TowerId"] = dtSale.Rows[0]["TowerId"].ToString();
                Session["CustomerId"] = dtSale.Rows[0]["CustomerID"].ToString();
                flatDetail.FlatNo = dtSale.Rows[0]["FlatName"].ToString();
                flatDetail.BlockName = dtSale.Rows[0]["BlockName"].ToString();
                if (dtSale.Rows[0]["SaleRate"].ToString() != "")
                {
                    flatDetail.SalePrice = Math.Round(Convert.ToDecimal(dtSale.Rows[0]["SaleRate"].ToString()), 0).ToString();
                    //lblServiceTax.Text = Math.Round(Convert.ToDecimal(dtSale.Rows[0]["ServiceTaxAmount"].ToString()), 0).ToString();
                    //lblTotal.Text = (Convert.ToDouble(txt_SalePrice.Text) + Convert.ToDouble(lblServiceTax.Text)).ToString();
                    // bad main dekhenge ddlInstallment.SelectedValue = (Math.Round(Convert.ToDecimal(dtSale.Rows[0]["Installments"].ToString()), 0).ToString() == "" ? "0" : Math.Round(Convert.ToDecimal(dtSale.Rows[0]["Installments"].ToString()), 0).ToString());

                    flatDetail.SaleRate = (Math.Round(Convert.ToDecimal(dtSale.Rows[0]["SaleRate"].ToString()), 0).ToString() == "" ? "0" : Math.Round(Convert.ToDecimal(dtSale.Rows[0]["SaleRate"].ToString()), 0).ToString());
                    flatDetail.InstallmentCount = (Math.Round(Convert.ToDecimal(dtSale.Rows[0]["Installments"].ToString()), 0).ToString() == "" ? "0" : Math.Round(Convert.ToDecimal(dtSale.Rows[0]["Installments"].ToString()), 0).ToString());
                    // bad main ddlInterval.SelectedValue = (Math.Round(Convert.ToDecimal(dtSale.Rows[0]["Interval"].ToString()), 0).ToString() == "" ? "0" : Math.Round(Convert.ToDecimal(dtSale.Rows[0]["Interval"].ToString()), 0).ToString());
                }
                else
                {
                    flatDetail.SaleRate = "0";
                    flatDetail.InstallmentCount = "0";
                    // bad main dekhenge GetService();
                }
            }
            l_FlatDetail.Add(flatDetail);
            return l_FlatDetail;
        }
        public JsonResult GetPaymentInstallment()
        {
            var paymentList = obj.GetDataTable("select InstallmentID,InstallmentNo from tblSInstallmentDetail where PayStatus=0 and RecordStatus=0 and SaleID=" + Convert.ToInt32(Session["SaleID"]) + " and FlatID=" + Convert.ToInt64(Session["FlatId"]) + " Order by InstallmentID asc");
            return Json(paymentList, JsonRequestBehavior.AllowGet);
        }

        // Check Flat Status
        public JsonResult IsFlatExists(string flatname, string pid)
        {
            DataTable dt = obj.GetDataTable("Select FlatName from tblsFlat where FlatName='" + flatname + "' and PID='" + pid + "' and Status=1");
            if (dt.Rows.Count > 0)
                return Json("Yes", JsonRequestBehavior.AllowGet);
            else
                return Json("No", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBanks()
        {
            List<BankDetails> lstBankDetails = new List<BankDetails>();
            DataTable dt = obj.GetDataTable("Select BankID,BankName from tblBank");
            foreach (DataRow bankDetails in dt.Rows)
            {
                lstBankDetails.Add(new BankDetails { BankID = Convert.ToInt32(bankDetails["BankID"]), BankName = Convert.ToString(bankDetails["BankName"]) });
            }
            return Json(lstBankDetails, JsonRequestBehavior.AllowGet);
        }
        // Get App Property List
        public JsonResult GetPropertyList()
        {
            List<PropertyListModel> lstPropertyDetails = new List<PropertyListModel>();
            DataTable dt = obj.GetDataTable("Select PID,PName from tblSProperty pt inner join UserProperty up on pt.PID=up.PropertyID where up.UserName='" + User.Identity.Name + "'");
            foreach (DataRow bankDetails in dt.Rows)
            {
                lstPropertyDetails.Add(new PropertyListModel { PropertyID = Convert.ToInt32(bankDetails["PID"]), PropertyName = Convert.ToString(bankDetails["PName"]) });
            }
            return Json(lstPropertyDetails, JsonRequestBehavior.AllowGet);
        }
        // Get App Property List
        public JsonResult GetPropertyTypeList(int pid)
        {
            List<PropertyListModel> lstPropertyDetails = new List<PropertyListModel>();
            DataTable dt = obj.GetDataTable("Select PropertyTypeID,PType from tblSPropertyType where PID='" + pid + "'");
            foreach (DataRow rw in dt.Rows)
            {
                lstPropertyDetails.Add(new PropertyListModel { PropertyID = Convert.ToInt32(rw["PropertyTypeID"].ToString()), PropertyName = rw["PType"].ToString() });
            }
            return Json(lstPropertyDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertPaymentDetailData(string InstallmentNo, string DueAmount, string DueDate, string paymentDate, string PayDate)
        {
            return new EmptyResult();
        }
        public EmptyResult InsertPaymentDetails(string InstallmentNo, string DueAmount, string DueDate, string PaymentMode, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, string Amtrcvdinwrds, string ReceivedAmount, string IsPrint, string IsEmailSent)
        {
            decimal TotalreceivedAmount = Convert.ToDecimal(ReceivedAmount);
            decimal totAmt = 0;
            decimal PayAmt = 0;
            decimal InterestAmount = 0;
            string PaymentNumber = "0";
            string MaxTransactionID = string.Empty;
            decimal TotalAmount = 0;
            MaxTransactionID = obj.GetMax("TransactionID", "tblSPayment") + 1;
            bool IsPrintReceipt = Convert.ToBoolean(IsPrint);
            bool IsSentEmail = Convert.ToBoolean(IsEmailSent);
            if (!String.IsNullOrEmpty(ReceivedAmount))
            {
                SavePayment(Convert.ToDecimal(DueAmount), InstallmentNo, InterestAmount, Convert.ToDecimal(ReceivedAmount), PaymentMode, Amtrcvdinwrds, PaymentNumber, MaxTransactionID, ChequeNo, ChequeDate, BankName, BankBranch, Remarks, PayDate, IsPrintReceipt, IsSentEmail);
            }

            return new EmptyResult();
        }

        protected void SavePaymentDetail(decimal DueAmount, string InstallmentNo, decimal InterestAmount, decimal PayAmount, string PaymentMode, String Amtrcvdinwrds, String PaymentNo, string TransactionID, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, bool IsPrint, bool IsEmailSent)
        {
            Hashtable htPayment = new Hashtable();
            htPayment.Add("InstallmentNo", InstallmentNo);
            htPayment.Add("SaleID", Session["SaleID"].ToString());
            htPayment.Add("PaymentDate", obj.Text_IndianDateFormat(PayDate));
            htPayment.Add("DueAmount", DueAmount);
            htPayment.Add("TotalAmount", PayAmount);
            htPayment.Add("Amount", PayAmount);
            htPayment.Add("PaymentMode", PaymentMode);
            htPayment.Add("Remarks", Remarks);
            htPayment.Add("AmtRcvdinWords", Amtrcvdinwrds);
            htPayment.Add("PaymentNo", PaymentNo);
            htPayment.Add("IsReceipt", IsPrint);
            htPayment.Add("Activity", "Add");
            htPayment.Add("TransactionID", TransactionID);
            htPayment.Add("FlatName", Session["FlatName"].ToString());
            if (PaymentMode == "1" || PaymentMode == "7")
            {
                htPayment.Add("PaymentStatus", "1");
            }
            else
            {
                htPayment.Add("PaymentStatus", "2");
                htPayment.Add("ChequeNo", ChequeNo);
                htPayment.Add("ChequeDate", Convert.ToDateTime(ChequeDate).Date);
                htPayment.Add("BankName", BankName);
                htPayment.Add("BankBranch", BankBranch);
            }
            //  htPayment.Add("CustomerName", lblCustomer.Text.Trim());
            if (obj.ExecuteProcedure("Insert_PaymentDetail", htPayment))
            {
                String MaxID = obj.GetMax("PaymentID", "tblSPaymentDetail");
                Session["MaxID"] = MaxID;
            }
        }
        protected void SavePayment(decimal DueAmount, string InstallmentNo, decimal InterestAmount, decimal PayAmount, string PaymentMode, String Amtrcvdinwrds, String PaymentNo, string TransactionID, string ChequeNo, string ChequeDate, string BankName, string BankBranch, string Remarks, string PayDate, bool IsPrint, bool IsEmailSent)
        {
            System.Globalization.DateTimeFormatInfo dtinfo = new System.Globalization.DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            int saleid = Convert.ToInt32(Session["SaleID"].ToString());
            Hashtable htPayment = new Hashtable();
            htPayment.Add("InstallmentNo", InstallmentNo);
            htPayment.Add("SaleID", saleid);
            htPayment.Add("PaymentDate", Convert.ToDateTime(PayDate, dtinfo));
            htPayment.Add("DueAmount", DueAmount);
            htPayment.Add("TotalAmount", DueAmount);
            htPayment.Add("Amount", PayAmount);
            htPayment.Add("PaymentMode", PaymentMode);
            if (PaymentMode == "1" || PaymentMode == "7")
            {
                htPayment.Add("PaymentStatus", "Clear");
            }
            else
            {
                htPayment.Add("PaymentStatus", "Pending");
                htPayment.Add("ChequeNo", ChequeNo);
                htPayment.Add("ChequeDate", Convert.ToDateTime(ChequeDate, dtinfo));
                htPayment.Add("BankName", BankName);
                htPayment.Add("BankBranch", BankBranch);
            }
            htPayment.Add("CustomerName", Session["CustomerName"].ToString());
            htPayment.Add("Remarks", Remarks);
            htPayment.Add("AmtRcvdinWords", Amtrcvdinwrds);
            htPayment.Add("Activity", "Add");
            htPayment.Add("PaymentNo", PaymentNo);
            htPayment.Add("IsReceipt", IsPrint);
            htPayment.Add("TransactionID", TransactionID);
            htPayment.Add("CustomerID", Convert.ToInt32("0"));
            htPayment.Add("CreatedBy", User.Identity.Name);
            htPayment.Add("FlatName", Session["FlatName"].ToString());
            if (obj.ExecuteProcedure("Insert_Payment", htPayment))
            {
            }
        }
        #region Not in Use Code

        protected void LedgerEntry(Int32 AccID, Int32 SaleID, Int32 FlatID, Decimal Amount, string InstallmentNo, Int32 TowerID, Int32 EventID)
        {
            string narr = "Installment Due For. " + FlatID + "";
            Hashtable ht = new Hashtable();
            ht.Add("AccID", AccID);
            ht.Add("SaleID", SaleID);
            ht.Add("InstallmentNo", InstallmentNo);
            ht.Add("FlatID", FlatID);
            //ht.Add("VDate", VDate);
            ht.Add("VType", "S");
            ht.Add("Narration", narr);
            ht.Add("Debit", Amount);
            ht.Add("Credit", 0);
            ht.Add("TowerID", TowerID);
            ht.Add("EventID", EventID);
            ht.Add("Remarks", "");
            ht.Add("Type", 1);
            ht.Add("Activity", "ADD");
            ht.Add("UserID", "123");// ((DataTable)Session["LoginDetails"]).Rows[0]["UserID"].ToString());
            obj.ExecuteProcedure("Insert_Ledger", ht);
        }
        protected void LedgerEntry(Int32 AccID, Int32 SaleID, Int32 FlatID, Decimal Amount, string InstallmentNo, Int32 TowerID, DateTime VDate)
        {
            string narr = "Installment Due For. " + FlatID + "";
            Hashtable ht = new Hashtable();
            ht.Add("AccID", AccID);
            ht.Add("SaleID", SaleID);
            ht.Add("InstallmentNo", InstallmentNo);
            ht.Add("FlatID", FlatID);
            ht.Add("VDate", VDate);
            ht.Add("VType", "S");
            ht.Add("Narration", narr);
            ht.Add("Debit", Amount);
            ht.Add("Credit", 0);
            ht.Add("TowerID", TowerID);
            ht.Add("Remarks", "");
            ht.Add("Type", 0);
            ht.Add("Activity", "ADD");
            ht.Add("UserID", "123123");// ((DataTable)Session["LoginDetails"]).Rows[0]["UserID"].ToString());

            obj.ExecuteProcedure("Insert_Ledger", ht);
        }
        protected void LedgerEntryClp(Int32 AccID, Int32 SaleID, Int32 FlatID, DateTime VDate, Decimal Amount, string InstallmentNo, Int32 EventID, Int32 TowerID, Int32 FloorNo, Int32 BID)
        {
            string narr = "Installment Due For. " + Convert.ToString(Session["FlatNo"]) + "";
            Hashtable ht = new Hashtable();
            ht.Add("AccID", AccID);
            ht.Add("SaleID", SaleID);
            ht.Add("InstallmentNo", InstallmentNo);
            ht.Add("FlatID", FlatID);
            ht.Add("VDate", VDate);
            ht.Add("VType", "S");
            ht.Add("Narration", narr);
            ht.Add("Debit", Amount);
            ht.Add("Credit", 0);
            ht.Add("Type", 0);
            ht.Add("EventID", EventID);
            ht.Add("TowerID", TowerID);
            ht.Add("FloorNo", FloorNo);
            ht.Add("BID", BID);
            ht.Add("UserID", Convert.ToInt32("123"));
            ht.Add("Activity", "ADD");
            ht.Add("Remarks", "");
            obj.ExecuteProcedure("Insert_Ledger", ht);
        }
        protected void LedgerEntryClp(Int32 AccID, Int32 SaleID, Int32 FlatID, Decimal Amount, string InstallmentNo, Int32 EventID, Int32 TowerID, Int32 FloorNo, Int32 BID)
        {
            string narr = "Installment Due For. " + Convert.ToString(Session["FlatNo"]) + "";
            Hashtable ht = new Hashtable();
            ht.Add("AccID", AccID);
            ht.Add("SaleID", SaleID);
            ht.Add("InstallmentNo", InstallmentNo);
            ht.Add("FlatID", FlatID);
            ht.Add("VType", "S");
            ht.Add("Narration", narr);
            ht.Add("Debit", Amount);
            ht.Add("Credit", 0);
            ht.Add("Type", 0);
            ht.Add("EventID", EventID);
            ht.Add("TowerID", TowerID);
            ht.Add("FloorNo", FloorNo);
            ht.Add("BID", BID);
            ht.Add("UserID", Convert.ToInt32(("123")));
            ht.Add("Activity", "ADD");
            ht.Add("Remarks", "");
            obj.ExecuteProcedure("Insert_Ledger", ht);
        }
        #endregion
    }
}

