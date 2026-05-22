using HealthcareMini.Data;
using HealthcareMini.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Services.UserServices
{
    public class UserServices : IUserServices
    {
        public readonly HealthcareDbContext _context;
        public UserServices(HealthcareDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        }
    }
}
