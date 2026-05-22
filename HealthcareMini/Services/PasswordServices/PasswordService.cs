using Org.BouncyCastle.Crypto.Generators;

namespace HealthcareMini.Services.PasswordServices
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            // Generate a salt with work factor 12 (good balance of security and performance)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
