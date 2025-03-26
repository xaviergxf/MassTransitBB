using MassTransit;

namespace MassTransitBB.ApiService.Events;
public record OrderSubmittedEvent(Guid OrderId) : CorrelatedBy<Guid>
{
    public Guid CorrelationId => OrderId;
}

public record OrderPayedEvent(Guid OrderId) : CorrelatedBy<Guid>
{
    public Guid CorrelationId => OrderId;
}

public record OrderDeliveredEvent(Guid OrderId) : CorrelatedBy<Guid>
{
    public Guid CorrelationId => OrderId;
}