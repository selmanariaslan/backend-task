using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Task.Core.Managers;
using Task.Core.Repositories;
using Task.Data;
using Task.Data.DatabaseContexts;

namespace Task.API.Bootstrapper
{
    public static class Ioc
    {
        public static void RegisterScopes(IServiceCollection services)
        {
            addDbContexts(services);
            addDependencies(services);
            //ConfigureGoogleServices(services);
        }
        private static void addDependencies(IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<LogManagementContext>, GenericRepository<LogManagementContext>>();
            services.AddScoped<IGenericRepository<TaskContext>, GenericRepository<TaskContext>>();
            services.AddScoped(typeof(IMemoryCache), typeof(MemoryCache));
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        private static void addDbContexts(IServiceCollection services)
        {
            var logManagementCnn = DatabaseConfig.GetConnectionString("LogManagement");
            services.AddDbContext<LogManagementContext>(options => options.UseNpgsql(logManagementCnn));

            var taskCnn = DatabaseConfig.GetConnectionString("TaskDb");
            services.AddDbContext<TaskContext>(options => options.UseNpgsql(taskCnn));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}