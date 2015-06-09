using AutoMapper;
using REMS.Data.Broker;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using REMS.Data.Access.Custo;

namespace REMS.Web.Areas.Customer.Controllers
{
    public class BrokerController : Controller
    {
        BrokerServices brokerServices=new BrokerServices();
        // GET: Customer/Broker  View Broker Payments
        [Authorize]
        public ActionResult AddNewBroker()
        {
            return View();
        }
        [Authorize]
        public ActionResult EditBroker(int id=0)
        {
            ViewBag.ID = id;
            return View();
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult AttachBroker(int Id=0)
        {
            ViewBag.ID = Id;
            return View();
        }
        [Authorize]
        public ActionResult PayBroker(int Id=0)
        {
            ViewBag.ID = Id;
            return View();
        }
        [Authorize]
        public ActionResult ApprovePayment(int Id=0)
        {
            ViewBag.ID = Id;
            return View();
        }
        [Authorize]
        public ActionResult PaymentLadger(int Id=0)
        {
            ViewBag.ID = Id;
            return View();
        }
        #region Broker_Services    
        public string SaveBroker(BrokerModel broker)
        {
            return brokerServices.SaveBroker(broker);
            
        }
        public int UpdateBroker(BrokerModel broker)
        {
            return brokerServices.UpdateBroker(broker);
        }
        public string SearchBroker(string search, string query)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.SearchBroker(search,query));
        }
        public string GetAllBroker()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetAllBroker());
        }
        public string GetBrokerBySaleID(int saleid)
        {
           return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokerBySaleID(saleid));
        }
        public string GetBrokerByFlatID(int flatid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokerByFlatID(flatid));
        }
        public string GetBrokerIdBySaleID(int saleid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokerIdBysaleId(saleid));
        }
        public string GetBrokerByBrokerToPropertyID(int pid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokerByBrokerToPropertyID(pid));         
        }
        public string GetBrokerByBrokerID(int bid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokerByBrokerID(bid)); 
        }
        public string GetBrokersProperties(int brokerid)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokersProperties(brokerid)); 
        }
        public string GetBrokersPropertiesSearch(int brokerid, string Status)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetBrokersPropertiesSearch(brokerid,Status)); 
        }
        public string GetPaidAmountToBrokerByBrokerID(int brokerID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetPaidAmountToBrokerByBrokerID(brokerID));         
        }
        public string GetPaidAmountToBrokerSearch(int brokerID, string Status)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.GetPaidAmountToBrokerSearch(brokerID, Status));
        }
        public string AttachBrokerToProperty(string brokerid, string amount, string date, string remarks,int FlatID, string SaleID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.AttachBrokerToProperty(brokerid,amount,date,remarks,FlatID,SaleID));
        }
        public string AttachBrokerToPropertyUpdate(int brokerToPropertyID, string brokerid, string amount, string date, string remarks, int PropertyID, int FlatID, string SaleID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.AttachBrokerToPropertyUpdate(brokerToPropertyID,brokerid,amount,date,remarks,PropertyID,FlatID,SaleID));
        }
        public string DeleteBrokerToPropertyID(int pid)
        {
           return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.DeleteBrokerToPropertyID(pid));
        }
        public string PaymentBrokerToProperty(int FlatID, int SaleID, int BrokerID, string PaidDate, string AmountPiad, int PID, string PaymentMode, string ChequeNo, string ChequeDate, string BankName, string BranchName, string Remarks)
        {
           return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.PaymentBrokerToProperty(FlatID,SaleID,BrokerID,PaidDate,AmountPiad,PID,PaymentMode,ChequeNo,ChequeDate,BankName,BranchName,Remarks));
        }
        public string UpdatePropertyStatusByID(int id, string status)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(brokerServices.UpdatePropertyStatusByID(id,status));
            
        }
        #endregion
    }
}