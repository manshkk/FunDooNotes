using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Entities
{
    public class Collaborator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NoteId { get; set; }

        [Required]
        public int OwnerUserId { get; set; }

        [Required]
        public int CollaboratorUserId { get; set; }

        public string Permission { get; set; } = "VIEW";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(NoteId))]
        public virtual Note Note { get; set; }

        [ForeignKey(nameof(CollaboratorUserId))]
        public virtual User CollaboratorUser { get; set; }
    }
}