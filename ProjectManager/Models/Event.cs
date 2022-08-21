using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProjectManager.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        
        [ForeignKey("EventTypes")]
        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }

        [ForeignKey("NTasks")]
        public int TaskId { get; set; }
        public NTask? Task { get; set; }

        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = String.Empty;
        public IdentityUser? User { get; set; }

        [Required]
        public int Value { get; set; } = 1;

        [Required]
        public DateTime TimeEntered { get; set; } = default;
    }
}
