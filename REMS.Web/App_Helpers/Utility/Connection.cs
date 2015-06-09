using System;
using System.Data;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


/// <summary>
/// Summary description for Connection
/// </summary>
public class Connection
{
	public Connection()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string ConnectionString()
    {
        string StrConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
        return StrConnection;
    }
}
