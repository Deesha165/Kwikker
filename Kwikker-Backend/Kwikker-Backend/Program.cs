using Hangfire;
using Kwikker_Backend.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using NLog;
using Service;
using Service.Contracts.Contracts;
using Service.DataShaping;
using Service.ServiceModels;
using Shared.DTOs;

namespace Kwikker_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configure logging
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            // 2. Add services to the container.
            builder.Services.AddSignalR();
            builder.Services.ConfigureRedis(builder.Configuration);

            builder.Services.AddAuthentication();
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJWT(builder.Configuration);

            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();

            builder.Services.AddScoped<ITrendService, TrendService>();
            builder.Services.ConfigureHangfire(builder.Configuration);
            builder.Services.AddHangfireServer(); // Hangfire service
            builder.Services.ConfigureCors(); // Ensure CORS policy is registered here
            builder.Services.ConfigureLoggerService();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.ConfigureSqlContext(builder.Configuration);

            builder.Services.AddScoped<IDataShaper<TweetDTO>, DataShaper<TweetDTO>>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 3. Build the application
            var app = builder.Build();

            // 4. Configure middleware
            app.UseExceptionHandler(opt => {
                // Global exception handling logic can go here
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use Hangfire Dashboard
            app.UseHangfireDashboard();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Use CORS Policy here, before routing and other middleware
            app.UseCors("CorsPolicy");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Controllers and SignalR Hub
            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");

            // Configure recurring jobs after building the app
            app.Services.ConfigureRecurringJobs();

            // Only one Hangfire Dashboard configuration is necessary
            // app.MapHangfireDashboard(); 

            // 5. Run the application
            app.Run();
        }
    }
}
