using System.Text.Json;
using ModelLayer.DTOs;
using BCrypt.Net;
using BusinessLayer.Interfaces;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly ITokenService _tokenService;

        private readonly IEmailService _emailService;

        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            IRabbitMQPublisher rabbitMQPublisher)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public bool Register(RegisterDTO registerDTO)
        {
            var existingUser =
                _userRepository.GetUserByEmail(
                    registerDTO.Email);

            if (existingUser != null)
            {
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
            }

            return result;
        }
        public string Login(LoginDTO loginDTO)
        {
            var user =
                _userRepository.GetUserByEmail(
                    loginDTO.Email);

            if (user == null)
            {
                return null;
            }

            bool passwordMatch =
                BCrypt.Net.BCrypt.Verify(
                    loginDTO.Password,
                    user.Password);

            if (!passwordMatch)
            {
                return null;
            }

            try
            {
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
                Console.WriteLine(ex.Message);
            }

            return _tokenService.GenerateToken(user);
        }
    }
}