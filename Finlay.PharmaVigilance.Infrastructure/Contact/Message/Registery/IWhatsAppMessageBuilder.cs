using Finlay.PharmaVigilance.Domain.Events;

namespace Finlay.PharmaVigilance.Infrastructure;

public interface IWhatsAppBuilder<T> where T : BasicEvent
{
    string Build(T eventData);
    bool CanHandle(Type eventType);
}