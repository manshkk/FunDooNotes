namespace ModelLayer.DTOs
{
    public class ResetPasswordDTO
    {
        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}