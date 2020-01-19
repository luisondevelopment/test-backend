using Microsoft.Extensions.DependencyInjection;
using TestBackend.Infrastructure;
using TestBackend.Service;

namespace TestBackend.Ioc
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<UserService>();
            services.AddTransient<UserRepository>();
        }
    }
}
