using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(University_Registration.Startup))]
namespace University_Registration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
