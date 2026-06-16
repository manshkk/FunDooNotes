using ModelLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        bool Register(RegisterDTO registerDTO);
        string Login(LoginDTO loginDTO);

        bool ForgotPassword(ForgotPasswordDTO dto);

        bool ResetPassword(
            string token,
            ResetPasswordDTO dto);
    }
}