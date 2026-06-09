using BusinessLayer.Interfaces;
using ModelLayer.DTOs;
using ModelLayer.DTOs.NoteDTOs;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<Note> CreateNoteAsync(int userId, CreateNoteDto dto)
        {
            return await _noteRepository.CreateNoteAsync(userId, dto);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId)
        {
            return await _noteRepository.GetAllNotesAsync(userId);
        }

        public async Task<Note?> GetNoteByIdAsync(int noteId, int userId)
        {
            return await _noteRepository
                .GetNoteByIdAsync(noteId, userId);
        }

        public async Task<Note?> UpdateNoteAsync(
            int noteId,
            int userId,
            UpdateNoteDto dto)
        {
            return await _noteRepository
                .UpdateNoteAsync(noteId, userId, dto);
        }

        public async Task<bool> MoveToTrashAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .MoveToTrashAsync(noteId, userId);
        }
        public async Task<IEnumerable<Note>> GetTrashedNotesAsync(int userId)
        {
            return await _noteRepository
                .GetTrashedNotesAsync(userId);
        }
        public async Task<bool> RestoreNoteAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .RestoreNoteAsync(noteId, userId);
        }
        public async Task<bool> PermanentDeleteAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .PermanentDeleteAsync(noteId, userId);
        }
        public async Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId)
        {
            return await _noteRepository
                .GetArchivedNotesAsync(userId);
        }
        public async Task<bool> ArchiveNoteAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .ArchiveNoteAsync(noteId, userId);
        }
        public async Task<bool> UnarchiveNoteAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .UnarchiveNoteAsync(noteId, userId);
        }
        public async Task<bool> PinNoteAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .PinNoteAsync(noteId, userId);
        }
        public async Task<bool> UnpinNoteAsync(
            int noteId,
            int userId)
        {
            return await _noteRepository
                .UnpinNoteAsync(noteId, userId);
        }
    }
}