namespace Order.Application.Contracts.Exception;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string name, object key)
        : base($"Entity \"{name}\" with {key} not found.")
    {
    }
}
