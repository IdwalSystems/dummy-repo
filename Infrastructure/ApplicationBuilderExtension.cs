using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Data;
using MSNK.SubscribeTableDependency;

namespace MSNK.Infrastructure
{
    public static class ApplicationBuilderExtension
    {

        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString)
            where T : ISubscribeTableDependency
        {

            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
            service?.SubscribeTableDependency(connectionString);
        }
    }
}
