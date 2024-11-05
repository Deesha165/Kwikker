using Contracts;
using Hangfire;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;
using Service.Contracts.Contracts;
using StackExchange.Redis;
using System.Runtime.CompilerServices;

namespace Kwikker_Backend.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:65418").AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("X-Pagination");
            });
            }
            );    
        }
        public static void ConfigureLoggerService(this IServiceCollection services) 
            => services.AddSingleton<ILoggerManager, LoggerManager>();
        public static void ConfigureRepositoryManager(this IServiceCollection services) 
            => services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) =>
services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,IConfiguration configuration) 
            =>services.AddDbContext<RepositoryContext>(opts =>opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), options => options.CommandTimeout(180)));
        public static void ConfigureRedis(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var x = configuration.GetConnectionString("redisConnection");
                var redisConfiguration = ConfigurationOptions.Parse(x);
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });
        }
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
            => services.AddHangfire(config => config.UseSqlServerStorage(configuration.GetConnectionString("sqlConnection")));
        public static void ConfigureRecurringJobs(this IServiceProvider services)
        {
            var recurringJobManager = services.GetRequiredService<IRecurringJobManager>();

            // Configure recurring job for updating trends every 
            recurringJobManager.AddOrUpdate<ITrendService>(
    "UpdateTrendsJob",
    service => service.GetTopTrends(),
              "*/2 * * * *");
        }
        
    }
}
