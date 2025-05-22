using MinimalMeet.Common.Events;

namespace MinimalMeet.Common.Interfaces;

public interface IEventProducer
{
    Task ProduceAsync<TEvent>(TEvent @event)
        where TEvent : EventBase;
}
