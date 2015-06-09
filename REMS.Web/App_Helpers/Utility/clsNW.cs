using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for clsNW
/// </summary>
public class clsNW1
{
	public clsNW1()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public  string rupees(decimal rupees)
    {
        string result = "";
        decimal res;
        if ((rupees / 10000000) > 0)
        {
            res = rupees / 10000000;
            rupees = rupees % 10000000;
            result = result + ' ' + rupeestowords(res) + " Crore";
        }
        if ((rupees / 100000) > 0)
        {
            res = rupees / 100000;
            rupees = rupees % 100000;
            result = result + ' ' + rupeestowords(res) + " Lakh";
        }
        if ((rupees / 1000) > 0)
        {
            res = rupees / 1000;
            rupees = rupees % 1000;
            result = result + ' ' + rupeestowords(res) + " Thousand";
        }
        if ((rupees / 100) > 0)
        {
            res = rupees / 100;
            rupees = rupees % 100;
            result = result + ' ' + rupeestowords(res) + " Hundred";
        }
        if ((rupees % 10) >= 0)
        {
            res = rupees % 100;
            result = result + " " + rupeestowords(res);
        }
        result = result + ' ' + "only/-";
        return result;
    }
    public string rupeestowords(decimal rupees)
    {
        string result = "";
        if ((rupees >= 1) && (rupees <= 10))
        {
            if ((rupees % 10) == 1) result = "One";
            if ((rupees % 10) == 2) result = "Two";
            if ((rupees % 10) == 3) result = "Three";
            if ((rupees % 10) == 4) result = "Four";
            if ((rupees % 10) == 5) result = "Five";
            if ((rupees % 10) == 6) result = "Six";
            if ((rupees % 10) == 7) result = "Seven";
            if ((rupees % 10) == 8) result = "Eight";
            if ((rupees % 10) == 9) result = "Nine";
            if ((rupees % 10) == 0) result = "Ten";
        }
        if (rupees > 9 && rupees <= 20)
        {
            if (rupees == 11) result = "Eleven";
            if (rupees == 12) result = "Twelve";
            if (rupees == 13) result = "Thirteen";
            if (rupees == 14) result = "Forteen";
            if (rupees == 15) result = "Fifteen";
            if (rupees == 16) result = "Sixteen";
            if (rupees == 17) result = "Seventeen";
            if (rupees == 18) result = "Eighteen";
            if (rupees == 19) result = "Nineteen";
            if (rupees == 20) result = "Twenty";
        }
        if (rupees > 20 && (rupees / 10) == 2 && (rupees % 10) == 0) result = "Twenty";
        if (rupees > 20 && (rupees / 10) == 3 && (rupees % 10) == 0) result = "Thirty";
        if (rupees > 20 && (rupees / 10) == 4 && (rupees % 10) == 0) result = "Forty";
        if (rupees > 20 && (rupees / 10) == 5 && (rupees % 10) == 0) result = "Fifty";
        if (rupees > 20 && (rupees / 10) == 6 && (rupees % 10) == 0) result = "Sixty";
        if (rupees > 20 && (rupees / 10) == 7 && (rupees % 10) == 0) result = "Seventy";
        if (rupees > 20 && (rupees / 10) == 8 && (rupees % 10) == 0) result = "Eighty";
        if (rupees > 20 && (rupees / 10) == 9 && (rupees % 10) == 0) result = "Ninty";

        if (rupees > 20 && (rupees / 10) == 2 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Twenty One";
            if ((rupees % 10) == 2) result = "Twenty Two";
            if ((rupees % 10) == 3) result = "Twenty Three";
            if ((rupees % 10) == 4) result = "Twenty Four";
            if ((rupees % 10) == 5) result = "Twenty Five";
            if ((rupees % 10) == 6) result = "Twenty Six";
            if ((rupees % 10) == 7) result = "Twenty Seven";
            if ((rupees % 10) == 8) result = "Twenty Eight";
            if ((rupees % 10) == 9) result = "Twenty Nine";
        }
        if (rupees > 20 && (rupees / 10) == 3 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Thirty One";
            if ((rupees % 10) == 2) result = "Thirty Two";
            if ((rupees % 10) == 3) result = "Thirty Three";
            if ((rupees % 10) == 4) result = "Thirty Four";
            if ((rupees % 10) == 5) result = "Thirty Five";
            if ((rupees % 10) == 6) result = "Thirty Six";
            if ((rupees % 10) == 7) result = "Thirty Seven";
            if ((rupees % 10) == 8) result = "Thirty Eight";
            if ((rupees % 10) == 9) result = "Thirty Nine";
        }
        if (rupees > 20 && (rupees / 10) == 4 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Forty One";
            if ((rupees % 10) == 2) result = "Forty Two";
            if ((rupees % 10) == 3) result = "Forty Three";
            if ((rupees % 10) == 4) result = "Forty Four";
            if ((rupees % 10) == 5) result = "Forty Five";
            if ((rupees % 10) == 6) result = "Forty Six";
            if ((rupees % 10) == 7) result = "Forty Seven";
            if ((rupees % 10) == 8) result = "Forty Eight";
            if ((rupees % 10) == 9) result = "Forty Nine";
        }
        if (rupees > 20 && (rupees / 10) == 5 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Fifty One";
            if ((rupees % 10) == 2) result = "Fifty Two";
            if ((rupees % 10) == 3) result = "Fifty Three";
            if ((rupees % 10) == 4) result = "Fifty Four";
            if ((rupees % 10) == 5) result = "Fifty Five";
            if ((rupees % 10) == 6) result = "Fifty Six";
            if ((rupees % 10) == 7) result = "Fifty Seven";
            if ((rupees % 10) == 8) result = "Fifty Eight";
            if ((rupees % 10) == 9) result = "Fifty Nine";
        }
        if (rupees > 20 && (rupees / 10) == 6 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Sixty One";
            if ((rupees % 10) == 2) result = "Sixty Two";
            if ((rupees % 10) == 3) result = "Sixty Three";
            if ((rupees % 10) == 4) result = "Sixty Four";
            if ((rupees % 10) == 5) result = "Sixty Five";
            if ((rupees % 10) == 6) result = "Sixty Six";
            if ((rupees % 10) == 7) result = "Sixty Seven";
            if ((rupees % 10) == 8) result = "Sixty Eight";
            if ((rupees % 10) == 9) result = "Sixty Nine";
        }
        if (rupees > 20 && (rupees / 10) == 7 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Seventy One";
            if ((rupees % 10) == 2) result = "Seventy Two";
            if ((rupees % 10) == 3) result = "Seventy Three";
            if ((rupees % 10) == 4) result = "Seventy Four";
            if ((rupees % 10) == 5) result = "Seventy Five";
            if ((rupees % 10) == 6) result = "Seventy Six";
            if ((rupees % 10) == 7) result = "Seventy Seven";
            if ((rupees % 10) == 8) result = "Seventy Eight";
            if ((rupees % 10) == 9) result = "Seventy Nine";
        }
        if (rupees > 20 && (rupees / 10) == 8 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Eighty One";
            if ((rupees % 10) == 2) result = "Eighty Two";
            if ((rupees % 10) == 3) result = "Eighty Three";
            if ((rupees % 10) == 4) result = "Eighty Four";
            if ((rupees % 10) == 5) result = "Eighty Five";
            if ((rupees % 10) == 6) result = "Eighty Six";
            if ((rupees % 10) == 7) result = "Eighty Seven";
            if ((rupees % 10) == 8) result = "Eighty Eight";
            if ((rupees % 10) == 9) result = "Eighty Nine";
        }
        if (rupees > 20 && (rupees / 10) == 9 && (rupees % 10) != 0)
        {
            if ((rupees % 10) == 1) result = "Ninty One";
            if ((rupees % 10) == 2) result = "Ninty Two";
            if ((rupees % 10) == 3) result = "Ninty Three";
            if ((rupees % 10) == 4) result = "Ninty Four";
            if ((rupees % 10) == 5) result = "Ninty Five";
            if ((rupees % 10) == 6) result = "Ninty Six";
            if ((rupees % 10) == 7) result = "Ninty Seven";
            if ((rupees % 10) == 8) result = "Ninty Eight";
            if ((rupees % 10) == 9) result = "Ninty Nine";
        }
        return result;
    }




}
