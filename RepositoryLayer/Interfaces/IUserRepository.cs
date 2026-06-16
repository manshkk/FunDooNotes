using ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        bool Register(User user);

        User GetUserByEmail(string email);

        bool UpdatePassword(
            string email,
            string newPassword);
    }
}