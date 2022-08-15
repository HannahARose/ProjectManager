using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class NTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
