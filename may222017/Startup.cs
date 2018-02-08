using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(may222017.Startup))]
namespace may222017
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
