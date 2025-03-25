

using Serilog;
using Survey_Basket;

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
            app.UseCors();
            app.UseAuthorization();


            app.MapControllers();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
