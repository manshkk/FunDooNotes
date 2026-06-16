using Microsoft.EntityFrameworkCore;
using ModelLayer.DTOs;
using ModelLayer.DTOs.NoteDTOs;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class NoteRepository : INoteRepository
    {
        private readonly FundooContext _context;

        private readonly ICacheService _cacheService;

        public NoteRepository(
            FundooContext context,
            ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<Note> CreateNoteAsync(int userId, CreateNoteDto dto)
        {
            var note = new Note
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                IsPinned = false,
                IsArchived = false,
                IsTrashed = false
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            _cacheService.RemoveData($"notes:user:{userId}");

            return note;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId)
        {
            string cacheKey = $"notes:user:{userId}";

            var cachedNotes =
                _cacheService.GetData<List<Note>>(cacheKey);

            if (cachedNotes != null)
            {
                Console.WriteLine("Cache Hit");

                return cachedNotes;
            }

            Console.WriteLine("Cache Miss");

            var notes = await _context.Notes
                .Where(n =>
                    n.UserId == userId &&
                    !n.IsTrashed)
                .ToListAsync();

            _cacheService.SetData(
                cacheKey,
                notes,
                DateTimeOffset.Now.AddMinutes(10));

            return notes;
        }

        public async Task<Note?> GetNoteByIdAsync(int noteId, int userId)
        {
            string cacheKey = $"note:{noteId}";

            var cachedNote =
                _cacheService.GetData<Note>(cacheKey);

            if (cachedNote != null)
            {
                Console.WriteLine("Note Cache Hit");

                return cachedNote;
            }

            Console.WriteLine("Note Cache Miss");

            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsTrashed);

            if (note != null)
            {
                _cacheService.SetData(
                    cacheKey,
                    note,
                    DateTimeOffset.Now.AddMinutes(10));
            }

            return note;
        }

        public async Task<Note?> UpdateNoteAsync(
            int noteId,
            int userId,
            UpdateNoteDto dto)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsTrashed);

            if (note == null)
                return null;

            note.Title = dto.Title;
            note.Description = dto.Description;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData($"notes:user:{userId}");

            return note;
        }

        public async Task<bool> MoveToTrashAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId);

            if (note == null)
                return false;

            note.IsTrashed = true;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
            $"notes:user:{userId}");

            return true;
        }
        public async Task<IEnumerable<Note>> GetTrashedNotesAsync(int userId)
        {
            return await _context.Notes
                .Where(n =>
                    n.UserId == userId &&
                    n.IsTrashed)
                .ToListAsync();
        }
        public async Task<bool> RestoreNoteAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    n.IsTrashed);

            if (note == null)
                return false;

            note.IsTrashed = false;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
            $"notes:user:{userId}");

            return true;
        }
        public async Task<bool> PermanentDeleteAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId);

            if (note == null)
                return false;

            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
            $"notes:user:{userId}");

            return true;
        }
        public async Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId)
        {
            return await _context.Notes
                .Where(n =>
                    n.UserId == userId &&
                    n.IsArchived &&
                    !n.IsTrashed)
                .ToListAsync();
        }
        public async Task<bool> ArchiveNoteAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsTrashed);

            if (note == null)
                return false;

            note.IsArchived = true;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
             $"notes:user:{userId}");

            return true;
        }
        public async Task<bool> UnarchiveNoteAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    n.IsArchived);

            if (note == null)
                return false;

            note.IsArchived = false;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
             $"notes:user:{userId}");

            return true;
        }
        public async Task<bool> PinNoteAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsTrashed);

            if (note == null)
                return false;

            note.IsPinned = true;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
             $"notes:user:{userId}");

            return true;
        }
        public async Task<bool> UnpinNoteAsync(
            int noteId,
            int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId);

            if (note == null)
                return false;

            note.IsPinned = false;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
             $"notes:user:{userId}");

            return true;
        }
        public async Task<bool> UpdateColorAsync(
            int noteId,
            int userId,
            string color)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsTrashed);

            if (note == null)
                return false;

            note.Color = color;
            note.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _cacheService.RemoveData(
             $"notes:user:{userId}");

            return true;
        }
    }
}

  