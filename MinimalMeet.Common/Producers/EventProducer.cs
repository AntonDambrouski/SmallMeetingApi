using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalMeet.Common.Configurations;
using MinimalMeet.Common.Events;
using MinimalMeet.Common.Interfaces;
using System.Text.Json;

namespace MinimalMeet.Common.Producers;

public class EventProducer : IEventProducer
{
    private readonly Lazy<ServiceBusSender> _serviceBusSender;
    private readonly ILogger<EventProducer> _logger;

    public EventProducer(IOptions<ServiceBusConfiguration> configs, ILogger<EventProducer> logger)
    {
        _logger = logger;

        var client = new ServiceBusClient(configs.Value.ConnectionString);
        _serviceBusSender = new Lazy<ServiceBusSender>(() => client.CreateSender(configs.Value.TopicName));
    }

    public async Task ProduceAsync<TEvent>(TEvent @event)
        where TEvent : EventBase
    {
        try
        {
            var eventMessage = JsonSerializer.Serialize(@event);
            var message = new ServiceBusMessage(eventMessage);

            await _serviceBusSender.Value.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while producing the event");
        }
    }
}
