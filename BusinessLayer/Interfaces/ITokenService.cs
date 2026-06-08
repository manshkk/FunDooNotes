using ModelLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}