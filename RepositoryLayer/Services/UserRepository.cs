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
    }
}