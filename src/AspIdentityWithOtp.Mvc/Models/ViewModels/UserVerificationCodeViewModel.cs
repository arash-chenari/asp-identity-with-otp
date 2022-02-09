using System.ComponentModel.DataAnnotations;

namespace AspIdentityWithOtp.Mvc.Models.ViewModels
{
    public class UserVerificationCodeViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
