using Domain.Database.Repositories;

namespace Domain.Database.Contracts;

public interface IAppUow : IBaseUOW
{
    public AppRefreshTokenRepository AppRefreshTokenRepository { get; }
}