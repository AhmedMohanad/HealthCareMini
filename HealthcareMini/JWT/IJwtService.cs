namespace HealthcareMini.JWT
{
    public interface IJwtService
    {

        public string GenerateJwtToken(int userId, string email, string role);
    }
}
