namespace Domain.Database.Contracts;


public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
where TEntity : class, IDomainEntity<Guid>
{
    
}

public interface IBaseRepository<TEntity, TKey>
    where TKey : struct
    where TEntity : class, IDomainEntity<TKey>
{
    public Task<TEntity?> FindAsync(TKey id);
    public TEntity Delete(TEntity entity);
    public Task<ICollection<TEntity>> AllAsync();
    public TEntity Update(TEntity entity);

    public Task<TEntity> AddAsync(TEntity entity);
}