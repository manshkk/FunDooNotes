using ModelLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO emailDTO);
    }
}