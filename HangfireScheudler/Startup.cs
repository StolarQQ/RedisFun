using Microsoft.Owin;
using HangfireFun;
using Owin;

[assembly: OwinStartup(typeof(HangfireScheudler.Startup))]
namespace HangfireScheudler
{
   public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HangfireConfiguration.HangfireInit("HangfireDb", app);
        }
    }
}