namespace Domain.Database.Contracts;

public interface IDomainEntityId : IDomainEntityId<Guid>
{
    
}

public interface IDomainEntityId<TKey>
where TKey : struct
{
    public TKey Id { get; set; }
}