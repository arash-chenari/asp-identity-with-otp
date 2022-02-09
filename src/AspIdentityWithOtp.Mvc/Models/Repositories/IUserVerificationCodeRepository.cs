using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspIdentityWithOtp.Mvc.Models.Repositories
{
    public interface IUserVerificationCodeRepository
    {
        Task<UserVerificationCode> GetLastVerificationCode(string userId);
        Task Add(UserVerificationCode verificationCodeModel);
    }

    public class UserVerificationCodeRepository : IUserVerificationCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public UserVerificationCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserVerificationCode> GetLastVerificationCode(string userId)
        {
            return await _context.UserVerificationCodes.Where(_ => _.UserId == userId)
                .OrderByDescending(_ => _.GeneratedCodeDateTime)
                .FirstOrDefaultAsync();

        }

        public async Task Add(UserVerificationCode verificationCodeModel)
        {
            _context.UserVerificationCodes.Add(verificationCodeModel);
            await _context.SaveChangesAsync();
        }
    }
}
