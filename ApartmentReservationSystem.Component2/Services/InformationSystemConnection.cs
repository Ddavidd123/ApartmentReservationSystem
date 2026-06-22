using System.ServiceModel;
using ApartmentReservationSystem.Shared.Contracts;

namespace ApartmentReservationSystem.Component2.Services;

public sealed class InformationSystemConnection : IDisposable
{
    private ChannelFactory<IInformationSystemService>? _factory;
    private IClientChannel? _channel;

    public IInformationSystemService? Service { get; private set; }

    public bool IsConnected => Service is not null;

    public string? Connect()
    {
        Disconnect();

        try
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                MaxReceivedMessageSize = 10 * 1024 * 1024
            };

            var endpoint = new EndpointAddress(WcfServiceConfiguration.ServiceUrl);
            _factory = new ChannelFactory<IInformationSystemService>(binding, endpoint);
            Service = _factory.CreateChannel();
            _channel = (IClientChannel)Service;

            _channel.Open();
            Service.GetApartments();

            return null;
        }
        catch (Exception ex)
        {
            Disconnect();
            return ex.Message;
        }
    }

    public void Disconnect()
    {
        if (_channel is { State: CommunicationState.Opened })
        {
            try
            {
                _channel.Close();
            }
            catch (CommunicationException)
            {
                _channel.Abort();
            }
        }

        _channel = null;
        Service = null;

        if (_factory is not null)
        {
            try
            {
                _factory.Close();
            }
            catch (CommunicationException)
            {
                _factory.Abort();
            }

            _factory = null;
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}
