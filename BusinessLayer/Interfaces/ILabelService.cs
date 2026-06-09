using ModelLayer.DTOs.LabelDTOs;
using ModelLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ILabelService
    {
        Task<Label> CreateLabelAsync(
            int userId,
            CreateLabelDto dto);

        Task<Label?> UpdateLabelAsync(
            int labelId,
            int userId,
            UpdateLabelDto dto);

        Task<bool> DeleteLabelAsync(
            int labelId,
            int userId);

        Task<bool> AddLabelToNoteAsync(
            int userId,
            AddLabelToNoteDto dto);

        Task<bool> RemoveLabelFromNoteAsync(
            int userId,
            AddLabelToNoteDto dto);
    }

}