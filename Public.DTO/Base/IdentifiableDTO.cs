using Domain.Database.Contracts;

namespace Public.DTO.Base;

public abstract class IdentifiableDTO : IDomainEntityId
{
    public Guid Id { get; set; }
}