using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models;

public class Project
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

