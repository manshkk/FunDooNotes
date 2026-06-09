using Microsoft.EntityFrameworkCore;
using ModelLayer.DTOs.LabelDTOs;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooContext _context;

        public LabelRepository(FundooContext context)
        {
            _context = context;
        }

        public async Task<Label> CreateLabelAsync(
            int userId,
            CreateLabelDto dto)
        {
            var label = new Label
            {
                LabelName = dto.LabelName,
                UserId = userId
            };

            _context.Labels.Add(label);

            await _context.SaveChangesAsync();

            return label;
        }
        public async Task<Label?> UpdateLabelAsync(
            int labelId,
            int userId,
            UpdateLabelDto dto)
        {
            var label = await _context.Labels
                .FirstOrDefaultAsync(l =>
                    l.LabelId == labelId &&
                    l.UserId == userId);

            if (label == null)
                return null;

            label.LabelName = dto.LabelName;

            await _context.SaveChangesAsync();

            return label;
        }
        public async Task<bool> DeleteLabelAsync(
            int labelId,
            int userId)
        {
            var label = await _context.Labels
                .FirstOrDefaultAsync(l =>
                    l.LabelId == labelId &&
                    l.UserId == userId);

            if (label == null)
                return false;

            _context.Labels.Remove(label);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AddLabelToNoteAsync(
            int userId,
            AddLabelToNoteDto dto)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == dto.NoteId &&
                    n.UserId == userId);

            if (note == null)
                return false;

            var label = await _context.Labels
                .FirstOrDefaultAsync(l =>
                    l.LabelId == dto.LabelId &&
                    l.UserId == userId);

            if (label == null)
                return false;

            bool exists = await _context.NoteLabels
                .AnyAsync(nl =>
                    nl.NoteId == dto.NoteId &&
                    nl.LabelId == dto.LabelId);

            if (exists)
                return true;

            var noteLabel = new NoteLabel
            {
                NoteId = dto.NoteId,
                LabelId = dto.LabelId
            };

            _context.NoteLabels.Add(noteLabel);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveLabelFromNoteAsync(
            int userId,
            AddLabelToNoteDto dto)
        {
            var noteLabel = await _context.NoteLabels
                .FirstOrDefaultAsync(nl =>
                    nl.NoteId == dto.NoteId &&
                    nl.LabelId == dto.LabelId);

            if (noteLabel == null)
                return false;

            _context.NoteLabels.Remove(noteLabel);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}