namespace Domain.Database.Contracts;

public abstract class DomainEntity : IDomainEntity
{
    public Guid Id { get; set; }
}

