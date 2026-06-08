using BCrypt.Net;
using BusinessLayer.Interfaces;
using ModelLayer.DTOs;
using ModelLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            return _userRepository.Register(user);
        }
    }
}