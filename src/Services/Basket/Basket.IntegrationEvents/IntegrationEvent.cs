namespace Basket.IntegrationEvents;

public abstract record IntegrationEvent
{
    public IntegrationEvent() 
        : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    protected IntegrationEvent(Guid id, DateTime createdAtUtc)
    {
        Id = id;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }
}