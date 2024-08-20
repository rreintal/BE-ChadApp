namespace Domain.Database.Contracts;

public interface IDomainEntity : IDomainEntity<Guid>
{
    
}

public interface IDomainEntity<TKey>
where TKey : struct
{
    public TKey Id { get; set; }
}