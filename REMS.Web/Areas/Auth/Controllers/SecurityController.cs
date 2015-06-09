using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Auth.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Auth/Security
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AssignPageToRole()
        {
            return View();
        }
        public ActionResult AddNewUser()
        {
            return View();
        }
        public ActionResult AssingRoles()
        {
            return View();
        }
        public ActionResult EditUserRole()
        {
            return View();
        }
        public ActionResult EditUserProperty()
        {
            return View();
        }
        public ActionResult ViewUsers()
        {
            return View();
        }
    }
}