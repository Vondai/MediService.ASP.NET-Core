using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediService.ASP.NET_Core;

namespace MediService.Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) 
            : base(configuration)
        {
        }
        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}
