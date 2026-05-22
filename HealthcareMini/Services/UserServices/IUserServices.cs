using HealthcareMini.Models.Entitys;

namespace HealthcareMini.Services.UserServices
{
    public interface IUserServices
    {


        public Task<User?> GetUserByEmailAsync(string email);
    }


}

