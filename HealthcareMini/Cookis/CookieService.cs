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


        // for login and register
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


        //for logout
        public void RemoveAuthCookie(HttpResponse response)
        {
            // Method 1: Simple delete (most common)
            response.Cookies.Delete("AuthToken");

            // OR Method 2: Delete with same options (more thorough)
            response.Cookies.Delete("AuthToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = _settings.Secure,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });
        }
    }
}
