using System.ComponentModel.DataAnnotations;

namespace AspIdentityWithOtp.Mvc.Models.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
