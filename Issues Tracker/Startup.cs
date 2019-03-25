using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Issues_Tracker.Startup))]
namespace Issues_Tracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
