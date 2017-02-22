using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Golf_6.Startup))]
namespace Golf_6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
