using ModelLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorService
    {
        bool AddCollaborator(
            int noteId,
            int ownerUserId,
            string email);

        List<User> GetCollaborators(
            int noteId);

        bool RemoveCollaborator(
            int noteId,
            int collaboratorUserId);

        List<Note> GetSharedNotes(
            int userId);
    }
}