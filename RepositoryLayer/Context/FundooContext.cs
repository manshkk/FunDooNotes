using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(
            DbContextOptions<FundooContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<NoteLabel> NoteLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NoteLabel>()
                .HasKey(nl => new
                {
                    nl.NoteId,
                    nl.LabelId
                });

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NoteId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabels)
                .HasForeignKey(nl => nl.LabelId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    
    }
}