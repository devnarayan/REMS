using AutoMapper;
using REMS.Data;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Customer.Controllers
{
    public class InfoController : Controller
    {
        //
        // GET: /Customer/Info/
        public ActionResult Index()
        {
            return View();
        }

        public string GetCustomerByCustID(string saleid)
        {
            REMSDBEntities context = new REMSDBEntities();
            int sid = Convert.ToInt32(saleid);
            var cust = context.Customers.Where(c => c.CustomerID == sid && c.SaleStatus.Value == true).FirstOrDefault();
            Mapper.CreateMap<REMS.Data.Customer,CustomerModel>();
            var model = Mapper.Map<REMS.Data.Customer, CustomerModel>(cust);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
	}
}