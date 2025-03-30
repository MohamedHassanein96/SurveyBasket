using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Serilog;

namespace SurveyBasket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddDependencies(builder.Configuration);

            builder.Host.UseSerilog((conext, configuration) =>

                    configuration.ReadFrom.Configuration(conext.Configuration)

               );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();



            app.UseHangfireDashboard("/Jobs", new DashboardOptions
            {
                Authorization =
                    [
                     new HangfireCustomBasicAuthenticationFilter
                     {
                         User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
                         Pass = app.Configuration.GetValue<string>("HangfireSettings:Password"),
                     }

                    ],
                DashboardTitle = "Survey Basket Dashboard",
                //IsReadOnlyFunc = (DashboardContext context) => true
            });



            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();



            RecurringJob.AddOrUpdate("SendNewPollsNotification", () => notificationService.SendNewPollsNotification(null), Cron.Daily);

            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
