using System.ComponentModel.DataAnnotations;
using Domain.Database.Contracts;

namespace Domain.Domain;

public class Exercise : DomainEntity
{
    public Guid? UserId { get; set; }

    public AppUser? User { get; set; }

    [Required] [MaxLength(100)] 
    public string Name { get; set; } = string.Empty;

    public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    public ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();   
}


