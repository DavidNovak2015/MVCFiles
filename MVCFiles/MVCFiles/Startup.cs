using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCFiles.Startup))]
namespace MVCFiles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
