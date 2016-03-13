using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Auth
{
    public interface IAuthorizeService
    {
        bool IsUserAuthorize(string username, string action, string controller, string area);
        List<UserModel> AuthUserList();
    }
   public  class AuthorizeService
    {
       #region Private Fields

       private readonly REMSDBEntities dbContext;

       #endregion
       public AuthorizeService()
       {
           dbContext = new REMSDBEntities();
       }
       public bool IsUserAuthorize(string username, string action, string controller, string area)
       {
            var model = dbContext.UserAccesses.Where(md => md.ControllerName == controller && md.ActionName == action && md.UserName == username).ToList();
            if (model.Count == 0)
                return false;
            else return true;
       }
        public List<UserModel> AuthUserList()
        {
            var model = dbContext.UserProperties.Where(md => md.IsAuthority == true).ToList();
            List<UserModel> uml = new List<UserModel>();
            foreach (var md in model)
            {
                uml.Add(new UserModel { UserName = md.UserName, Property = md.ProjectID.ToString() });
            }
            return uml;
        }
    }
}
