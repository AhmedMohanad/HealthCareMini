using Microsoft.Extensions.Options;

namespace HealthcareMini.Cookis
{
    public class CookieService : ICookieService
    {

        private readonly CookieSettings _settings; // bound from appsettings.json

        public CookieService(IOptions<CookieSettings> settings)
        {
            _settings = settings.Value;
        }

        public void AppendAuthCookie(HttpResponse response, string token)
        {

            response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = _settings.Secure,        
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                Path = "/",
                IsEssential = true
            });
        }
    }
}
