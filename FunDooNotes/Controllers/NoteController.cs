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
        [HttpGet("trash")]
        public async Task<IActionResult> GetTrashedNotes()
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var notes = await _noteService
                .GetTrashedNotesAsync(userId);

            return Ok(new
            {
                Success = true,
                Message = "Trashed notes retrieved successfully",
                Data = notes
            });
        }
        [HttpPatch("{noteId}/restore")]
        public async Task<IActionResult> RestoreNote(
            int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .RestoreNoteAsync(noteId, userId);

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
                Message = "Note restored successfully"
            });
        }
        [HttpDelete("{noteId}/permanent")]
        public async Task<IActionResult> PermanentDelete(
               int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .PermanentDeleteAsync(noteId, userId);

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
                Message = "Note permanently deleted"
            });
        }
        [HttpGet("archive")]
        public async Task<IActionResult> GetArchivedNotes()
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .GetArchivedNotesAsync(userId);

            return Ok(new
            {
                Success = true,
                Message = "Archived notes retrieved successfully",
                Data = result
            });
        }
        [HttpPatch("{noteId}/archive")]
        public async Task<IActionResult> ArchiveNote(int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .ArchiveNoteAsync(noteId, userId);

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
                Message = "Note archived successfully"
            });
        }
        [HttpPatch("{noteId}/unarchive")]
        public async Task<IActionResult> UnarchiveNote(int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .UnarchiveNoteAsync(noteId, userId);

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
                Message = "Note unarchived successfully"
            });
        }
        [HttpPatch("{noteId}/pin")]
        public async Task<IActionResult> PinNote(int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .PinNoteAsync(noteId, userId);

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
                Message = "Note pinned successfully"
            });
        }
        [HttpPatch("{noteId}/unpin")]
        public async Task<IActionResult> UnpinNote(int noteId)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .UnpinNoteAsync(noteId, userId);

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
                Message = "Note unpinned successfully"
            });
        }
        [HttpPatch("{noteId}/color")]
        public async Task<IActionResult> UpdateColor(
                int noteId,
                UpdateColorDto dto)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = await _noteService
                .UpdateColorAsync(
                    noteId,
                    userId,
                    dto.Color);

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
                Message = "Color updated successfully"
            });
        }
    }
}