using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Surseed.Startup))]
namespace Surseed
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
