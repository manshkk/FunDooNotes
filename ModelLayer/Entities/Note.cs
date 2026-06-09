using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Entities
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsPinned { get; set; } = false;

        public bool IsArchived { get; set; } = false;

        public bool IsTrashed { get; set; } = false;

        public string? Color { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<NoteLabel> NoteLabels { get; set; } = new List<NoteLabel>();
    }
}