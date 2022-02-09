using System.ComponentModel.DataAnnotations;

namespace AspIdentityWithOtp.Mvc.Models.ViewModels
{
    public class VerifyUserRegistrationViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Code { get; set; }
    }
}
