using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentReservationSystem.Component1.Wcf;

public class WcfHostService
{
    private IHost? _host;

    public const string BaseAddress = "http://localhost:8733";
    public const string ServiceUrl = "http://localhost:8733/InformationSystem/service";

    public void Start(InformationSystemService serviceImplementation)
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(BaseAddress);
                webBuilder.ConfigureServices(services =>
                {
                    services.AddServiceModelServices();
                    services.AddServiceModelMetadata();
                    services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
                    services.AddSingleton(serviceImplementation);
                });
                webBuilder.Configure(app =>
                {
                    app.UseServiceModel(serviceBuilder =>
                    {
                        serviceBuilder.AddService<InformationSystemService>();
                        serviceBuilder.AddServiceEndpoint<InformationSystemService, Shared.Contracts.IInformationSystemService>(
                            new BasicHttpBinding(),
                            "/InformationSystem/service");
                    });
                });
            })
            .Build();

        _host.Start();
    }

    public void Stop()
    {
        if (_host is null)
        {
            return;
        }

        _host.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();
        _host = null;
    }
}
