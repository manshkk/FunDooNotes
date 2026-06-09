using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.DTOs.NoteDTOs;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService.CreateNoteAsync(userId, dto);

            return Ok(new
            {
                Success = true,
                Message = "Note Created Successfully",
                Data = result
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService.GetAllNotesAsync(userId);

            return Ok(new
            {
                Success = true,
                Message = "Notes Retrieved Successfully",
                Data = result
            });
        }
        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNoteById(int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var note = await _noteService
                .GetNoteByIdAsync(noteId, userId);

            if (note == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Note not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Data = note
            });
        }
        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(
            int noteId,
            UpdateNoteDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .UpdateNoteAsync(noteId, userId, dto);

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Note not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Note Updated Successfully",
                Data = result
            });
        }
        [HttpDelete("{noteId}")]
        public async Task<IActionResult> MoveToTrash(int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .MoveToTrashAsync(noteId, userId);

            if (!result)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Note not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Note moved to trash successfully"
            });
        }
    }
}