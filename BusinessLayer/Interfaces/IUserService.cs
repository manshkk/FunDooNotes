using ModelLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        bool Register(RegisterDTO registerDTO);
        string Login(LoginDTO loginDTO);
    }
}