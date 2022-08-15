using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set;}

        [Required]
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        [Required]
        public int ValueMultiplier { get; set; } = 1;
    }
}

