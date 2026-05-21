namespace HealthcareMini.JWT
{
    public interface IJwtService
    {

        public string GenerateJwtToken(string email, string role);
    }
}
