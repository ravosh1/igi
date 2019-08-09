using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testgithubproject.Startup))]
namespace testgithubproject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
