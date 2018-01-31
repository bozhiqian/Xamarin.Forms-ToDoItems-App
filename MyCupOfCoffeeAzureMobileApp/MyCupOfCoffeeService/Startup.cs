using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyCupOfCoffeeService.Startup))]

namespace MyCupOfCoffeeService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}