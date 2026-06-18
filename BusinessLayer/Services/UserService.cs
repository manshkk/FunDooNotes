using BCrypt.Net;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOs;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly ITokenService _tokenService;

        private readonly IEmailService _emailService;

        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        private readonly IConfiguration _configuration;

        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            IRabbitMQPublisher rabbitMQPublisher,
            IConfiguration configuration,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _rabbitMQPublisher = rabbitMQPublisher;
            _configuration = configuration;
            _logger = logger;
        }

        public bool Register(RegisterDTO registerDTO)
        {
            _logger.LogInformation(
            "Registration attempt for Email {Email}",
            registerDTO.Email);
            var existingUser =
                _userRepository.GetUserByEmail(
                    registerDTO.Email);

            if (existingUser != null)
            {
                _logger.LogWarning(
                    "Registration failed. Email {Email} already exists",
                    registerDTO.Email);

                return false;
            }

            User user = new User();

            user.FirstName = registerDTO.FirstName;
            user.LastName = registerDTO.LastName;
            user.Email = registerDTO.Email;

            user.Password =
                BCrypt.Net.BCrypt.HashPassword(
                    registerDTO.Password);

            user.CreatedAt = DateTime.UtcNow;
            user.ChangedAt = DateTime.UtcNow;

            bool result = _userRepository.Register(user);

            if (result)
            {
                var emailMessage = new EmailMessageDTO
                {
                    Email = user.Email,
                    FirstName = user.FirstName
                };

                string message =
                    JsonSerializer.Serialize(emailMessage);

                _rabbitMQPublisher.Publish(
                    "fundoo.email.queue",
                    message);

                _logger.LogInformation(
                    "Welcome email message published to RabbitMQ for Email {Email}",
                    user.Email);
            }

            return result;
        }
        public string Login(LoginDTO loginDTO)
        {

            _logger.LogInformation(
                "Login attempt for Email {Email}",
                loginDTO.Email);
            var user =
                _userRepository.GetUserByEmail(
                    loginDTO.Email);

            if (user == null)
            {
                _logger.LogWarning(
                    "Login failed. User not found for Email {Email}",
                    loginDTO.Email);

                return null;
            }

            bool passwordMatch =
                BCrypt.Net.BCrypt.Verify(
                    loginDTO.Password,
                    user.Password);

            if (!passwordMatch)
            {
                _logger.LogWarning(
                    "Login failed. Invalid password for Email {Email}",
                    loginDTO.Email);

                return null;
            }

            try
            {
                _logger.LogInformation(
                    "Login notification email sent successfully to Email {Email}",
                    user.Email);

                _emailService.SendEmail(
                    new EmailDTO
                    {
                        ToEmail = user.Email,

                        Subject = "Welcome Back to Fundoo Notes",

                        Body =
                        $@"
                        <div style='font-family:Arial,sans-serif;padding:20px'>
                            <h2 style='color:#4CAF50'>
                                Welcome Back, {user.FirstName}!
                            </h2>

                            <p>
                                We noticed a successful login to your Fundoo Notes account.
                            </p>

                            <p>
                                <strong>Login Time:</strong> {DateTime.Now}
                            </p>

                            <p>
                                We're glad to see you again. Your notes and important information are ready for you.
                            </p>

                            <p>
                                If this login was not performed by you, please change your password immediately.
                            </p>

                            <br/>

                            <p>Happy Note Taking! 📝</p>

                            <p>
                                Regards,<br/>
                                <strong>Fundoo Notes Team</strong>
                            </p>
                        </div>"
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send login notification email to Email {Email}",
                    user.Email);
            }
            _logger.LogInformation(
                "User logged in successfully for Email {Email}",
                user.Email);

            return _tokenService.GenerateToken(user);
        }

        public bool ForgotPassword(ForgotPasswordDTO dto)
        {
            var user =
                _userRepository.GetUserByEmail(
                    dto.Email);

            if (user == null)
            {
                return false;
            }

            var tokenHandler =
                new JwtSecurityTokenHandler();

            var key =
                Encoding.UTF8.GetBytes(
                    "FundooNotesProjectJWTAuthenticationSecretKey2026");

            var tokenDescriptor =
                new SecurityTokenDescriptor
                {
                    Subject =
                        new ClaimsIdentity(
                        new[]
                        {
                    new Claim(
                        ClaimTypes.Email,
                        user.Email)
                        }),

                    Expires =
                        DateTime.UtcNow.AddMinutes(30),

                    SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                };

            var token =
                tokenHandler.CreateToken(
                    tokenDescriptor);

            string resetToken =
                tokenHandler.WriteToken(token);

            var forgotPasswordMessage =
                new ForgotPasswordMessageDTO
                {
                    Email = user.Email,
                    ResetToken = resetToken
                };

            string message =
                JsonSerializer.Serialize(
                    forgotPasswordMessage);

            _rabbitMQPublisher.Publish(
                "fundoo.forgotpassword.queue",
                message);

            return true;
        }

        public bool ResetPassword(
            string token,
            ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return false;
            }

            var tokenHandler =
                new JwtSecurityTokenHandler();

            var key =
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]);

            try
            {
                var principal =
                    tokenHandler.ValidateToken(
                        token,
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(key),

                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ClockSkew = TimeSpan.Zero
                        },
                        out SecurityToken validatedToken);

                string email =
                    principal.FindFirst(
                        ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }

                string hashedPassword =
                    BCrypt.Net.BCrypt.HashPassword(
                        dto.NewPassword);

                return _userRepository.UpdatePassword(
                    email,
                    hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}