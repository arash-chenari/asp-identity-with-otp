using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspIdentityWithOtp.Mvc.Models.Repositories
{
    public interface ITempUserRegistrationRepository
    {
        Task Add(TempUserRegistration model);
        Task<TempUserRegistration> GetLastByPhoneNumber(string phoneNumber);
    }

    public class TempUserRegistrationRepository : ITempUserRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public TempUserRegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(TempUserRegistration model)
        {
            _context.TempUserRegistrations.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<TempUserRegistration> GetLastByPhoneNumber(string phoneNumber)
        {
            return await _context.TempUserRegistrations
                .Where(_ => _.PhoneNumber == phoneNumber)
                .OrderByDescending(_ => _.GeneratedCodeDateTime)
                .FirstOrDefaultAsync();
        }
    }
}
