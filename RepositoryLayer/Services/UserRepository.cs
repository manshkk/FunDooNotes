using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly FundooContext _context;

        public UserRepository(FundooContext context)
        {
            _context = context;
        }

        public bool Register(User user)
        {
            _context.Users.Add(user);

            _context.SaveChanges();

            return true;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public bool UpdatePassword(
            string email,
            string newPassword)
        {
            var user =
                _context.Users
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;

            user.ChangedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return true;
        }
    }
}