using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FunDooNotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(
        IUserService userService,
        IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            try
            {
                bool result =
                    _userService.Register(registerDTO);

                if (result)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "User Registered Successfully"
                    });
                }

                return BadRequest(new
                {
                    Success = false,
                    Message = "Email already exists"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                string token =
                    _userService.Login(loginDTO);

                if (token == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid Credentials"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new
            {
                Message = "Authenticated User Access Granted"
            });
        }
        [HttpGet("test-email")]
        public IActionResult TestEmail()
        {
            _emailService.SendEmail(
                new EmailDTO
                {
                    ToEmail = "manishdn2003@gmail.com",
                    Subject = "Fundoo Notes SMTP Test",
                    Body =
                    "<h2>SMTP Working Successfully</h2>" +
                    "<p>This is a test email from Fundoo Notes.</p>"
                });

            _emailService.SendEmail(
                new EmailDTO
                {
                    ToEmail = "manishkaushal0334@gmail.com",
                    Subject = "Fundoo Notes SMTP Test",
                    Body =
                    "<h2>SMTP Working Successfully</h2>" +
                    "<p>This is a test email from Fundoo Notes.</p>"
                });

            return Ok(new
            {
                Success = true,
                Message = "Emails Sent Successfully"
            });
        }
    }
}