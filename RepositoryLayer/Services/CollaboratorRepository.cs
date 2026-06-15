using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly FundooContext _context;

        public CollaboratorRepository(FundooContext context)
        {
            _context = context;
        }

        public bool AddCollaborator(int noteId, int ownerUserId, string email)
        {
            var note = _context.Notes
                .FirstOrDefault(n =>
                    n.NoteId == noteId &&
                    n.UserId == ownerUserId);

            if (note == null)
                return false;

            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
                return false;

            var existingCollaborator = _context.Collaborators
                .FirstOrDefault(c =>
                    c.NoteId == noteId &&
                    c.CollaboratorUserId == user.UserId);

            if (existingCollaborator != null)
                return false;

            Collaborator collaborator = new Collaborator()
            {
                NoteId = noteId,
                OwnerUserId = ownerUserId,
                CollaboratorUserId = user.UserId,
                Permission = "VIEW"
            };

            _context.Collaborators.Add(collaborator);
            _context.SaveChanges();

            return true;
        }

        public List<User> GetCollaborators(int noteId)
        {
            return _context.Collaborators
                .Where(c => c.NoteId == noteId)
                .Select(c => c.CollaboratorUser)
                .ToList();
        }

        public bool RemoveCollaborator(int noteId, int collaboratorUserId)
        {
            var collaborator = _context.Collaborators
                .FirstOrDefault(c =>
                    c.NoteId == noteId &&
                    c.CollaboratorUserId == collaboratorUserId);

            if (collaborator == null)
                return false;

            _context.Collaborators.Remove(collaborator);
            _context.SaveChanges();

            return true;
        }

        public List<Note> GetSharedNotes(int userId)
        {
            return _context.Collaborators
                .Where(c => c.CollaboratorUserId == userId)
                .Select(c => c.Note)
                .ToList();
        }
    }
}