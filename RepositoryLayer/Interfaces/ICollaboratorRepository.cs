using ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepository
    {
        bool AddCollaborator(int noteId, int ownerUserId, string email);

        bool RemoveCollaborator(int noteId, int collaboratorUserId);

        List<User> GetCollaborators(int noteId);

        List<Note> GetSharedNotes(int userId);
    }
}