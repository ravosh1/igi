using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YourWorldWithin.Startup))]
namespace YourWorldWithin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
