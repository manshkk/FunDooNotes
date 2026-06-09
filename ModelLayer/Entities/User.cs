using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ChangedAt { get; set; }

        public ICollection<Note> Notes { get; set; }

        public ICollection<Label> Labels { get; set; } = new List<Label>();
    }
}