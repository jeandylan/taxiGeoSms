using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(taxiGeoSms.Startup))]
namespace taxiGeoSms
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
