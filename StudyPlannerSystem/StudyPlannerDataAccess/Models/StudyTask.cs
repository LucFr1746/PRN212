using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyPlannerDataAccess.Models;

[Table("StudyTask")]
public class StudyTask
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int SubjectId { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? DayOfWeek { get; set; }

    public bool IsCompleted { get; set; }

    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("SubjectId")]
    public virtual Subject Subject { get; set; } = null!;
}
