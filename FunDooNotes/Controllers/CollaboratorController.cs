using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorService _service;

        public CollaboratorController(
            ICollaboratorService service)
        {
            _service = service;
        }

        [HttpPost("{noteId}")]
        public IActionResult AddCollaborator(
            int noteId,
            string email)
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var result = _service.AddCollaborator(
                noteId,
                userId,
                email);

            if (!result)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Unable to add collaborator"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Collaborator added successfully"
            });
        }

        [HttpGet("{noteId}")]
        public IActionResult GetCollaborators(
            int noteId)
        {
            var collaborators =
                _service.GetCollaborators(noteId);

            return Ok(new
            {
                Success = true,
                Data = collaborators
            });
        }

        [HttpDelete("{noteId}/{collaboratorUserId}")]
        public IActionResult RemoveCollaborator(
            int noteId,
            int collaboratorUserId)
        {
            var result =
                _service.RemoveCollaborator(
                    noteId,
                    collaboratorUserId);

            if (!result)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Collaborator not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Collaborator removed successfully"
            });
        }

        [HttpGet("shared")]
        public IActionResult GetSharedNotes()
        {
            int userId = Convert.ToInt32(
                User.FindFirst("UserId")?.Value);

            var notes =
                _service.GetSharedNotes(userId);

            return Ok(new
            {
                Success = true,
                Data = notes
            });
        }
    }
}