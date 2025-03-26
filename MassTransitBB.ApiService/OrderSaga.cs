using MassTransit;
using MassTransitBB.ApiService.Events;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public required string CurrentState { get; set; }
    public required DateTimeOffset OrderSubmittedDate { get; set; }
    public DateTimeOffset? OrderPayedDate { get; set; }
    public DateTimeOffset? OrderDeliveredDate { get; set; }
}

public class OrderSaga : MassTransitStateMachine<OrderState>
{
    public Event<OrderSubmittedEvent>? SubmitOrder { get; private set; }
    public Event<OrderPayedEvent>? OrderPayed { get; private set; }
    public Event<OrderDeliveredEvent>? OrderDelivered { get; private set; }

    public State? Submitted { get; private set; }
    public State? Payed { get; private set; }
    public State? Delivered { get; private set; }

    public OrderSaga()
    {
        InstanceState(p => p.CurrentState);

        Initially(
            When(SubmitOrder)
                .Then(c => c.Saga.OrderSubmittedDate = DateTimeOffset.UtcNow)
                .TransitionTo(Submitted));
        During(Submitted,
            When(OrderPayed)
                .Then(c => c.Saga.OrderPayedDate = DateTimeOffset.Now)
                .TransitionTo(Payed).Publish(o => new OrderDeliveredEvent(o.Saga.CorrelationId)));
        During(Payed, 
            When(OrderDelivered)
                .Then(c => c.Saga.OrderDeliveredDate = DateTimeOffset.Now)
                .TransitionTo(Delivered));
    }
}

public class OrderStateMap :
    SagaClassMap<OrderState>
{
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
    }
}
