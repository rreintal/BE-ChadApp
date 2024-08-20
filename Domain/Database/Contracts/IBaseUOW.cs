namespace Domain.Database.Contracts;

public interface IBaseUOW
{
    public Task<int> SaveChangesAsync();
}