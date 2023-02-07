using System;
using System.Net;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly;

[assembly: FunctionsStartup(typeof(Vnext.Function.Startup))]
namespace Vnext.Function
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IDeviceAsset, DeviceAsset>();
            builder.Services.AddHttpClient<IDeviceAsset, DeviceAsset>().AddTransientHttpErrorPolicy(
                policy => policy.OrResult(r => r.StatusCode == HttpStatusCode.BadRequest).WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(2))

            );
        }
    }





}