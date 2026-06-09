using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.LabelDTOs;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(
            ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel(
            CreateLabelDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _labelService
                .CreateLabelAsync(userId, dto);

            return Ok(new
            {
                Success = true,
                Message = "Label created successfully",
                Data = result
            });
        }
        [HttpPut("{labelId}")]
        public async Task<IActionResult> UpdateLabel(
            int labelId,
            UpdateLabelDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _labelService
                .UpdateLabelAsync(
                    labelId,
                    userId,
                    dto);

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Label not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Label updated successfully",
                Data = result
            });
        }
        [HttpDelete("{labelId}")]
        public async Task<IActionResult> DeleteLabel( int labelId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _labelService
                .DeleteLabelAsync(
                    labelId,
                    userId);

            if (!result)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Label not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Label deleted successfully"
            });
        }
        [HttpPost("add-to-note")]
        public async Task<IActionResult> AddLabelToNote(AddLabelToNoteDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _labelService
                .AddLabelToNoteAsync(userId, dto);

            if (!result)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid Note or Label"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Label added to note successfully"
            });
        }
        [HttpDelete("remove-from-note")]
        public async Task<IActionResult> RemoveLabelFromNote(AddLabelToNoteDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _labelService
                .RemoveLabelFromNoteAsync(userId, dto);

            if (!result)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Mapping not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Label removed from note successfully"
            });
        }
    }
}