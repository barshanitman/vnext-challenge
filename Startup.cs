using System;
using System.Net;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly;
using Vnext.Function.Entities;

[assembly: FunctionsStartup(typeof(Vnext.Function.Startup))]
namespace Vnext.Function
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<DeviceContext>(
                       options => options.UseSqlServer("Server=tcp:in2m5hcujmphc.database.windows.net,1433;Initial Catalog=DevicesDB;Persist Security Info=False;User ID=alohauser;Password=!Aloha27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            builder.Services.AddTransient<IDeviceAsset, DeviceAsset>();
            builder.Services.AddHttpClient<IDeviceAsset, DeviceAsset>().AddTransientHttpErrorPolicy(
                policy => policy.OrResult(r => r.StatusCode == HttpStatusCode.BadRequest).WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(0.5))

            );
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            builder.Services.AddDbContext<DeviceContext>(
            options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));


        }

    }
}


