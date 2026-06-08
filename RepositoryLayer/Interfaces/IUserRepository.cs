using ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        bool Register(User user);
    }
}