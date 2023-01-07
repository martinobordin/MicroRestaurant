namespace Order.Domain.Shared;
public abstract class Entity
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } 
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
