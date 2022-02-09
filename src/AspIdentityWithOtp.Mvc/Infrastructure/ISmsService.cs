namespace AspIdentityWithOtp.Mvc.Infrastructure
{
    public interface ISmsService
    {
        void Send(string phoneNumber, string code);
    }

    public class SmsService : ISmsService
    {
        public void Send(string phoneNumber, string code)
        {
            //send sms with your sms service
        }
    }
}
