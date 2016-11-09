using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SurveyApp.Startup))]
namespace SurveyApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
