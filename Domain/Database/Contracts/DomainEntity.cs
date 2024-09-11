using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Database.Contracts;

public abstract class DomainEntity : IDomainEntityId, IDomainEntityTimestamp
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}

