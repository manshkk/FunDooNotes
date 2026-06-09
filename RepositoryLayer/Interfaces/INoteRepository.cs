using ModelLayer.DTOs;
using ModelLayer.DTOs.NoteDTOs;
using ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRepository
    {
        Task<Note> CreateNoteAsync(int userId, CreateNoteDto dto);

        Task<IEnumerable<Note>> GetAllNotesAsync(int userId);

        Task<Note?> GetNoteByIdAsync(int noteId, int userId);

        Task<Note?> UpdateNoteAsync(
            int noteId,
            int userId,
            UpdateNoteDto dto);

        Task<bool> MoveToTrashAsync(
            int noteId,
            int userId);

        Task<IEnumerable<Note>> GetTrashedNotesAsync(int userId);

        Task<bool> RestoreNoteAsync(int noteId, int userId);

        Task<bool> PermanentDeleteAsync(
            int noteId,
            int userId);

        Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId);

        Task<bool> ArchiveNoteAsync(int noteId, int userId);

        Task<bool> UnarchiveNoteAsync(int noteId, int userId);

        Task<bool> PinNoteAsync(int noteId, int userId);

        Task<bool> UnpinNoteAsync(int noteId, int userId);

        Task<bool> UpdateColorAsync(
                int noteId,
                int userId,
                string color);
    }
}