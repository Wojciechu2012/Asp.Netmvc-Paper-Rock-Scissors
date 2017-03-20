using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Paper_Rock.Startup))]
namespace Paper_Rock
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
