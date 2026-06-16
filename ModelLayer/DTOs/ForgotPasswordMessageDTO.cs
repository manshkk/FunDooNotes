namespace ModelLayer.DTOs
{
    public class ForgotPasswordMessageDTO
    {
        public string Email { get; set; } = string.Empty;

        public string ResetToken { get; set; } = string.Empty;
    }
}