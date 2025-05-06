using System.ComponentModel.DataAnnotations;

namespace MangoApi.MangoModels
{
    public class UserResetPasswordRequest
    {
        [Required]
        public string username { get; set; }
    }



    public class UserResetPasswordint
    {
        public string? Email { get; set; }

        public string? TemporaryPassword { get; set; }

        public string? NewPassword { get; set; }
    }
}
