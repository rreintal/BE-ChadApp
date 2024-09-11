namespace Domain.Database.Contracts;

public interface IDomainEntityTimestamp
{
    public DateTime CreatedAt { get; set; }
}