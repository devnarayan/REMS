using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using REMS.Data;
using REMS.Data.Access;
using REMS.Data.Access.Admin;
using REMS.Data.Access.Auth;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using REMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace REMS.Web.Areas.Admin.Controllers
{
    public class SecurityController : Controller
    {
        #region Private Fields
        private ProjectService projectService;
        private Helper hp;
        private REMSDBEntities dbContext;
        private AuthorizeService authService;
        #endregion
        public SecurityController()
        {
            hp = new Helper();
            projectService = new ProjectService();
            dbContext = new REMSDBEntities();
            authService = new AuthorizeService();
        }
        // GET: Admin/Security
        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult AddNewUser()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult EditUserRole()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult EditUserProperty()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult ViewUsers()
        {
            return View();
        }
        // [MyAuthorize]
        public ActionResult AssingRoles()
        {
            return View();
        }

        public ActionResult AssignPageToRole()
        {
            return View();
        }
        #region Security Services
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public async Task<string> AddUser(UserModel model)
        {
            string[] pids = model.Property.Split(',');
            if (pids.Length > 0)
            {

                var user = new ApplicationUser() { UserName = model.UserName, Email = model.EmailID };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Roles.AddUserToRole(model.UserName, model.RoleName);
                    //  await SignInAsync(user, isPersistent: false);
                    // Add Users Property
                    REMSDBEntities context = new REMSDBEntities();
                    foreach (string pid in pids)
                    {
                        UserProperty up = new UserProperty();
                        up.UserName = model.UserName;
                        up.ProjectID = Convert.ToInt32(pid);
                        up.AssignUser = User.Identity.Name;
                        up.AssignDate = DateTime.Now;
                        context.UserProperties.Add(up);
                        int i = context.SaveChanges();
                    }
                    var roleacc = context.RoleAccesses.Where(ro => ro.RoleName == model.RoleName).ToList();
                    List<RoleAccess> ra = new List<RoleAccess>();

                    foreach (var r in roleacc)
                    {
                        ra.Add(r);
                    }
                    foreach (var ro in ra)
                    {
                        //ra.Add(ro);
                        UserAccess us = new UserAccess();
                        us.UserName = model.UserName;
                        us.IsRead = true; us.IsWrite = true;
                        us.ControllerName = ro.ControllerName;
                        us.ActionName = ro.ActionName;
                        us.AssignDate = DateTime.Now;
                        us.AssignUser = User.Identity.Name;
                        us.ModuleListID = ro.ModuleListID;
                        context.UserAccesses.Add(us);
                        int i = context.SaveChanges();
                    }
                }
                if (result.Succeeded)
                {
                    return "User Added Successfully.";
                }
                else
                {
                    string st = "";
                    foreach (var er in result.Errors)
                    {
                        st += er.ToString();
                    }
                    return st;
                }
            }
            else
            {
                return "Please select any one Property for User";
            }
        }
        public JsonResult GetPropertyList()
        {
            ProjectService pservice = new ProjectService();
            var model = pservice.AllProjects();
            List<ProjectListModel> lstPropertyDetails = new List<ProjectListModel>();
            foreach (var plist in model)
            {
                lstPropertyDetails.Add(new ProjectListModel { ProjectID = plist.ProjectID, ProjectName = plist.PName });
            }
            return Json(lstPropertyDetails, JsonRequestBehavior.AllowGet);
        }
        public string GetModuleListByUser(string username)
        {
            REMSDBEntities context = new REMSDBEntities();
            DataFunctions obj = new DataFunctions();
            List<ModuleListModel> lstPropertyDetails = new List<ModuleListModel>();
            DataTable dt = obj.GetDataTable("select * from ModuleList ");
            string st = "";
            foreach (DataRow bankDetails in dt.Rows)
            {
                string controller = bankDetails["Controller"].ToString();
                string action = bankDetails["ActionName"].ToString();
                bool isread = false, iswrite = false; string chk = "";
                var UAM = context.UserAccesses.Where(ua => ua.UserName == username && ua.ControllerName == controller && ua.ActionName == action).FirstOrDefault();
                if (UAM != null)
                {


                    isread = UAM.IsRead;
                    iswrite = UAM.IsWrite;
                    if (isread) chk = "checked";
                }
                lstPropertyDetails.Add(new ModuleListModel { ModuleListID = Convert.ToInt32(bankDetails["ModuleListID"]), Name = Convert.ToString(bankDetails["Name"]), PageName = bankDetails["PageName"].ToString(), Controller = bankDetails["Controller"].ToString(), ActionName = bankDetails["ActionName"].ToString(), UserName = username, IsRead = isread, IsWrite = iswrite, Checked = chk });
                st += @"<tr><td><input type='checkbox' id='" + Convert.ToInt32(bankDetails["ModuleListID"]) + "'  value='" + Convert.ToInt32(bankDetails["ModuleListID"]) + "' name='chkPro' class='Prolist' " + chk + "/></td>";
                st += @"<td><input type='checkbox' id='Auth" + Convert.ToInt32(bankDetails["ModuleListID"]) + "'  value='" + Convert.ToInt32(bankDetails["ModuleListID"]) + "' name='chkAuth' class='ProAuth' " + chk + "/></td>";
                st += "<td>" + Convert.ToString(bankDetails["Name"]) + "</td><td>" + Convert.ToString(bankDetails["PageName"]) + "</td><td>" + Convert.ToString(bankDetails["Controller"]) + "</td><td>" + Convert.ToString(bankDetails["ActionName"]) + "</td></tr>";
            }
            return st;
        }
        public string SaveUsersRights(string username, string modulelist, string notinaccess)
        {
            try
            {
                REMSDBEntities context = new REMSDBEntities();
                string[] pids = modulelist.Split(',');
                if (pids.Length > 0)
                {
                    foreach (string pid in pids)
                    {
                        var UAM = context.UserAccesses.Where(ua => ua.UserName == username && ua.ModuleListID == pid).FirstOrDefault();
                        if (UAM == null)
                        {
                            // Insert
                            int mid = Convert.ToInt32(pid);
                            var Mdl = context.ModuleLists.Where(md => md.ModuleListID == mid).FirstOrDefault();
                            UserAccess us = new UserAccess();
                            us.UserName = username;
                            us.IsRead = true; us.IsWrite = true;
                            us.ControllerName = Mdl.Controller;
                            us.ActionName = Mdl.ActionName;
                            us.AssignDate = DateTime.Now;
                            us.AssignUser = User.Identity.Name;
                            us.ModuleListID = pid;
                            context.UserAccesses.Add(us);
                            int i = context.SaveChanges();
                        }
                        else
                        {
                            // Update
                            UAM.IsRead = true;
                            UAM.IsWrite = true;
                            UAM.AssignUser = User.Identity.Name;
                            UAM.AssignDate = DateTime.Now;
                            context.UserAccesses.Add(UAM);
                            context.Entry(UAM).State = EntityState.Modified;
                            int i = context.SaveChanges();
                        }
                    }
                }
                string[] llist = notinaccess.Split(',');
                foreach (string pid in llist)
                {
                    var UAM = context.UserAccesses.Where(ua => ua.UserName == username && ua.ModuleListID == pid).FirstOrDefault();
                    if (UAM == null)
                    {
                    }
                    else
                    {
                        // Delete
                        context.UserAccesses.Add(UAM);
                        context.Entry(UAM).State = EntityState.Deleted;
                        int i = context.SaveChanges();
                    }
                }
                return "Users Access Rights Saved.";
            }

            catch (Exception ex)
            {
                hp.LogException(ex);
                return "Error!, Please try again.";
            }
        }
        public string GetPropertyByUser(string username)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = projectService.AllProjects();
            DataFunctions obj = new DataFunctions();
            List<ProjectListModel> lstPropertyDetails = new List<ProjectListModel>();

            DataTable dt = obj.GetDataTable("select * from Project");
            string st = "";
            foreach (var mdl in model)
            {
                string chk = "", chk2 = "";
                var UAM = context.UserProperties.Where(ua => ua.UserName == username && ua.ProjectID == mdl.ProjectID).FirstOrDefault();
                if (UAM != null)
                {
                    chk = "checked";
                    if (UAM.IsAuthority == true)
                        chk2 = "checked";
                }

                // lstPropertyDetails.Add(new ModuleListModel { ModuleListID = Convert.ToInt32(bankDetails["ModuleListID"]), Name = Convert.ToString(bankDetails["Name"]), PageName = bankDetails["PageName"].ToString(), Controller = bankDetails["Controller"].ToString(), ActionName = bankDetails["ActionName"].ToString(), UserName = username, IsRead = isread, IsWrite = iswrite, Checked = chk });
                st += @"<tr><td><input type='checkbox' id='" + Convert.ToInt32(mdl.ProjectID) + "'  value='" + mdl.ProjectID + "' name='chkPro' class='Prolist' " + chk + "/></td>";
                st += @"<td><input type='checkbox'  id='chk" + Convert.ToInt32(mdl.ProjectID) + "'  value='" + mdl.ProjectID + "' name='chkAuth' class='ProAuth' " + chk2 + "/></td>";
                st += "<td>" + mdl.PName + "</td></tr>";
            }
            return st;
        }
        public string GetPropertyByProjectID(int projectid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = projectService.AllProjects();
            DataFunctions obj = new DataFunctions();
            List<ProjectListModel> lstPropertyDetails = new List<ProjectListModel>();

            string st = "";
            int i = 0;
            var UAM = context.UserProperties.Where(ua => ua.ProjectID == projectid && ua.IsAuthority==true).ToList();
            foreach (var mdl in UAM)
            {
                i++;
                st += @"<tr><td>" + i + "</td>";
                st += @"<td><input type='checkbox' class='form-control ProAuth' id='chk" + Convert.ToInt32(mdl.UserPropertyID) + "'  value='" + mdl.UserName + "' name='chkAuth'/></td>";
                st+="<td>" + mdl.UserName + "</td></td>";
                st += @"<td><select class='form-control mydays' id='" + i + "' name='"+mdl.UserName+"'><option value='2'>2 Days</option><option value='1'>1 Day</option><option value='3'>3 Days</option><option value='4'>4 Days</option><option value='5'>5 Days</option><option value='6'>6 Days</option><option value='7'>7 Days</option></select></td>";
                st += "</tr>";
            }
            return st;
        }
        public string SaveUsersPropertyRights(string username, string modulelist, string notinaccess, string IsAuth, string NAuth)
        {
            try
            {
                string[] pids = modulelist.Split(',');
                if (pids.Length > 0)
                {
                    foreach (string pid in pids)
                    {
                        int? proid = Convert.ToInt32(pid);
                        var UAM = dbContext.UserProperties.Where(ua => ua.UserName == username && ua.ProjectID == proid).FirstOrDefault();
                        if (UAM == null)
                        {
                            // Insert
                            int mid = Convert.ToInt32(pid);
                            var Mdl = dbContext.ModuleLists.Where(md => md.ModuleListID == mid).FirstOrDefault();
                            UserProperty us = new UserProperty();
                            us.UserName = username;
                            us.ProjectID = proid;
                            us.AssignDate = DateTime.Now;
                            us.AssignUser = User.Identity.Name;
                            dbContext.UserProperties.Add(us);
                            int i = dbContext.SaveChanges();
                        }
                        else
                        {
                            // Update
                            UAM.AssignDate = DateTime.Now;
                            UAM.AssignUser = User.Identity.Name;
                            dbContext.UserProperties.Add(UAM);
                            dbContext.Entry(UAM).State = EntityState.Modified;
                            int i = dbContext.SaveChanges();
                        }
                    }
                }
                string[] llist = notinaccess.Split(',');
                if (llist.Count() > 0)
                {
                    foreach (string pid in llist)
                    {
                        int? proid = Convert.ToInt32(pid);

                        var UAM = dbContext.UserProperties.Where(ua => ua.UserName == username && ua.ProjectID == proid).FirstOrDefault();
                        if (UAM == null)
                        {
                        }
                        else
                        {
                            // Delete 
                            dbContext.UserProperties.Add(UAM);
                            dbContext.Entry(UAM).State = EntityState.Deleted;
                            int i = dbContext.SaveChanges();
                        }
                    }
                }
                string[] iauth = IsAuth.Split(',');
                if (iauth.Length > 0)
                {
                    foreach (string pid in iauth)
                    {
                        int? proid = Convert.ToInt32(pid);
                        var UAM = dbContext.UserProperties.Where(ua => ua.UserName == username && ua.ProjectID == proid).FirstOrDefault();
                        if (UAM == null)
                        {

                        }
                        else
                        {
                            // Update
                            UAM.AssignDate = DateTime.Now;
                            UAM.AssignUser = User.Identity.Name;
                            UAM.IsAuthority = true;
                            dbContext.UserProperties.Add(UAM);
                            dbContext.Entry(UAM).State = EntityState.Modified;
                            int i = dbContext.SaveChanges();
                        }
                    }
                }
                string[] nauth = NAuth.Split(',');
                if (nauth.Count() > 0)
                {
                    foreach (string pid in nauth)
                    {
                        int? proid = Convert.ToInt32(pid);

                        var UAM = dbContext.UserProperties.Where(ua => ua.UserName == username && ua.ProjectID == proid).FirstOrDefault();
                        if (UAM == null)
                        {
                        }
                        else
                        {
                            UAM.AssignDate = DateTime.Now;
                            UAM.AssignUser = User.Identity.Name;
                            UAM.IsAuthority = false;
                            dbContext.UserProperties.Add(UAM);
                            dbContext.Entry(UAM).State = EntityState.Modified;
                            int i = dbContext.SaveChanges();
                        }
                    }
                }
                return "Users Propety Access Rights Saved.";
            }

            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return "Error!, Please try again.";
            }
        }

        // Role Access Start
        public string GetModuleListByRole(string rolename)
        {
            DataFunctions obj = new DataFunctions();
            List<ModuleListModel> lstPropertyDetails = new List<ModuleListModel>();
            DataTable dt = obj.GetDataTable("select * from ModuleList ");
            string st = "";
            foreach (DataRow bankDetails in dt.Rows)
            {
                string controller = bankDetails["Controller"].ToString();
                string action = bankDetails["ActionName"].ToString();
                bool isread = false, iswrite = false; string chk = "";
                var UAM = dbContext.RoleAccesses.Where(ua => ua.RoleName == rolename && ua.ControllerName == controller && ua.ActionName == action).FirstOrDefault();
                if (UAM != null)
                {
                    isread = UAM.IsRead;
                    if (isread) chk = "checked";
                }
                // lstPropertyDetails.Add(new ModuleListModel { ModuleListID = Convert.ToInt32(bankDetails["ModuleListID"]), Name = Convert.ToString(bankDetails["Name"]), PageName = bankDetails["PageName"].ToString(), Controller = bankDetails["Controller"].ToString(), ActionName = bankDetails["ActionName"].ToString(), UserName = rolename, IsRead = isread, IsWrite = iswrite, Checked = chk });
                st += @"<tr><td><input type='checkbox' id='" + Convert.ToInt32(bankDetails["ModuleListID"]) + "'  value='" + Convert.ToInt32(bankDetails["ModuleListID"]) + "' name='chkPro' class='Prolist' " + chk + "/></td>";
                st += "<td>" + Convert.ToString(bankDetails["Name"]) + "</td><td>" + Convert.ToString(bankDetails["PageName"]) + "</td><td>" + Convert.ToString(bankDetails["Controller"]) + "</td><td>" + Convert.ToString(bankDetails["ActionName"]) + "</td></tr>";
            }
            return st;
        }
        public string SaveRoleRights(string rolename, string modulelist, string notinaccess)
        {
            try
            {
                string[] pids = modulelist.Split(',');
                if (pids.Length > 0)
                {
                    foreach (string pid in pids)
                    {
                        var UAM = dbContext.RoleAccesses.Where(ua => ua.RoleName == rolename && ua.ModuleListID == pid).FirstOrDefault();
                        if (UAM == null)
                        {
                            // Insert
                            int mid = Convert.ToInt32(pid);
                            var Mdl = dbContext.ModuleLists.Where(md => md.ModuleListID == mid).FirstOrDefault();
                            RoleAccess us = new RoleAccess();
                            us.RoleName = rolename;
                            us.IsRead = true;
                            us.ControllerName = Mdl.Controller;
                            us.ActionName = Mdl.ActionName;
                            us.AssignDate = DateTime.Now;
                            us.AssignUser = User.Identity.Name;
                            us.ModuleListID = pid;
                            dbContext.RoleAccesses.Add(us);
                            int i = dbContext.SaveChanges();
                        }
                        else
                        {
                            // Update
                            UAM.IsRead = true;
                            UAM.AssignUser = User.Identity.Name;
                            UAM.AssignDate = DateTime.Now;
                            dbContext.RoleAccesses.Add(UAM);
                            dbContext.Entry(UAM).State = EntityState.Modified;
                            int i = dbContext.SaveChanges();
                        }
                    }
                }
                string[] llist = notinaccess.Split(',');
                foreach (string pid in llist)
                {
                    var UAM = dbContext.RoleAccesses.Where(ua => ua.RoleName == rolename && ua.ModuleListID == pid).FirstOrDefault();
                    if (UAM == null)
                    {
                    }
                    else
                    {
                        // Delete
                        dbContext.RoleAccesses.Add(UAM);
                        dbContext.Entry(UAM).State = EntityState.Deleted;
                        int i = dbContext.SaveChanges();
                    }
                }
                return "Users Access Rights Saved.";
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return "Error!, Please try again.";
            }
        }

        public JsonResult GetUserList()
        {
            try
            {
                // UserManager.PasswordHasher.
                DataFunctions obj = new DataFunctions();
                List<UserModel> lstPropertyDetails = new List<UserModel>();
                DataTable dt = obj.GetDataTable("select UserName from AspNetUsers");

                foreach (DataRow bankDetails in dt.Rows)
                {
                    lstPropertyDetails.Add(new UserModel { UserName = Convert.ToString(bankDetails["UserName"]) });
                }
                return Json(lstPropertyDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetUsersInfoList()
        {
            try
            {
                // UserManager.PasswordHasher.
                DataFunctions obj = new DataFunctions();
                List<UserModel> lstPropertyDetails = new List<UserModel>();
                DataTable dt = obj.GetDataTable("select u.UserName,au.Email,r.roleName,au.PasswordHash from aspnet_users u inner join aspnet_usersinroles us on u.UserID=us.UserID inner join aspnet_roles r on us.RoleId=r.RoleID inner join aspnetusers au on u.username=au.username");

                foreach (DataRow bankDetails in dt.Rows)
                {
                    lstPropertyDetails.Add(new UserModel { UserName = Convert.ToString(bankDetails["UserName"]), EmailID = bankDetails["Email"].ToString(), RoleName = bankDetails["RoleName"].ToString() });
                }
                return Json(lstPropertyDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public string GetUserRole(string username)
        {
            string[] role = Roles.GetRolesForUser(username);
            if (role.Count() > 0)
                return role[0];
            else return "User";
        }
        public string GetUersAccess(string UserName)
        {

            var user = dbContext.UserAccesses.Where(ua => ua.UserName == UserName).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(user);
        }
        public string UpdateUserRole(string username, string OldRoleName, string NewRoleName)
        {
            try
            {
                Roles.RemoveUserFromRole(username, OldRoleName);
                Roles.AddUserToRole(username, NewRoleName);
                return "Role Updated Successfully.";
            }
            catch (Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return "Error! Please try again.";
            }
        }
        public string AuthUserList()
        {
            try
            {
               var model= authService.AuthUserList();
               return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch(Exception ex)
            {
                Helper hp = new Helper();
                hp.LogException(ex);
                return "Error! Please try again.";
            }
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}