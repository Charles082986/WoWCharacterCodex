using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WoWCharacterCodex.Web.Startup))]
namespace WoWCharacterCodex.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
