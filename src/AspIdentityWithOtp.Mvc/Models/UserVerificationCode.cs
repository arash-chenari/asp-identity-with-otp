using System;

namespace AspIdentityWithOtp.Mvc.Models
{
    public class UserVerificationCode
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime GeneratedCodeDateTime { get; set; }
    }
}
