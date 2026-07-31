using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyPlannerDataAccess.Models;

[Table("Student")]
public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(20)]
    public string StudentCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<StudyTask> StudyTasks { get; set; } = new List<StudyTask>();
}
