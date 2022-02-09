using Microsoft.AspNetCore.Identity;

namespace AspIdentityWithOtp.Mvc.Models
{
    public class User : IdentityUser
    {
        //This is for sending sms to different countries
        public string CountryCallingCode { get; set; }
    }
}
