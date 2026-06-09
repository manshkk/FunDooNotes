using BusinessLayer.Interfaces;
using ModelLayer.DTOs.LabelDTOs;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(
            ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task<Label> CreateLabelAsync(
            int userId,
            CreateLabelDto dto)
        {
            return await _labelRepository
                .CreateLabelAsync(userId, dto);
        }
        public async Task<Label?> UpdateLabelAsync(
            int labelId,
            int userId,
            UpdateLabelDto dto)
        {
            return await _labelRepository
                .UpdateLabelAsync(
                    labelId,
                    userId,
                    dto);
        }
        public async Task<bool> DeleteLabelAsync(
                int labelId,
                int userId)
        {
            return await _labelRepository
                .DeleteLabelAsync(
                    labelId,
                    userId);
        }
        public async Task<bool> AddLabelToNoteAsync(
            int userId,
            AddLabelToNoteDto dto)
        {
            return await _labelRepository
                .AddLabelToNoteAsync(userId, dto);
        }
        public async Task<bool> RemoveLabelFromNoteAsync(
            int userId,
            AddLabelToNoteDto dto)
        {
            return await _labelRepository
                .RemoveLabelFromNoteAsync(userId, dto);
        }
    }
}