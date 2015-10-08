using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Auth0DNC.Startup))]
namespace Auth0DNC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
