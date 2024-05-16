using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AmarisTestApp.DataAccess;
using AmarisTestApp.BusinessLogic;
using AmarisTestApp.Controllers;

namespace AmarisTestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Registra EmployeeApiClient como un servicio
                    services.AddScoped<EmployeeApiClient>();
                    services.AddScoped<IEmployeeController, EmployeeController>();
                });
    }
}
