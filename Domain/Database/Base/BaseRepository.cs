using Domain.Database.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database.Base;

public class BaseRepository<TDBContext, TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IDomainEntityId<Guid>
    where TDBContext : AppDbContext
{

    protected TDBContext _context;
    protected DbSet<TEntity> DbSet;
    public BaseRepository(TDBContext context)
    {
        _context = context;
        DbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> FindAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual TEntity Delete(TEntity entity)
    {
        return DbSet.Remove(entity).Entity;
    }

    public virtual async Task<ICollection<TEntity>> AllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual TEntity Update(TEntity entity)
    {
        return DbSet.Update(entity).Entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        return (await DbSet.AddAsync(entity)).Entity;
    }
}