using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CBUSA.Startup))]
namespace CBUSA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
