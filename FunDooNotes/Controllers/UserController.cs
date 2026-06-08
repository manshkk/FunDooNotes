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

        public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}