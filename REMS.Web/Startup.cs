using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(REMS.Web.Startup))]
namespace REMS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
