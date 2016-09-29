using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EAITestApplication.Startup))]
namespace EAITestApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
