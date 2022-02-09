using System;

namespace AspIdentityWithOtp.Mvc.Models
{
    public class TempUserRegistration
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public DateTime GeneratedCodeDateTime { get; set; }
    }
}
