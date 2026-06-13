using BusinessLayer.Interfaces;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class CollaboratorService : ICollaboratorService
    {
        private readonly ICollaboratorRepository _repository;

        public CollaboratorService(
            ICollaboratorRepository repository)
        {
            _repository = repository;
        }

        public bool AddCollaborator(
            int noteId,
            int ownerUserId,
            string email)
        {
            return _repository.AddCollaborator(
                noteId,
                ownerUserId,
                email);
        }

        public List<User> GetCollaborators(
            int noteId)
        {
            return _repository
                .GetCollaborators(noteId);
        }

        public bool RemoveCollaborator(
            int noteId,
            int collaboratorUserId)
        {
            return _repository
                .RemoveCollaborator(
                    noteId,
                    collaboratorUserId);
        }

        public List<Note> GetSharedNotes(
            int userId)
        {
            return _repository
                .GetSharedNotes(userId);
        }
    }
}