using Microsoft.Extensions.Options;

namespace HealthcareMini.Cookis
{
    public interface ICookieService
    {

        public void AppendAuthCookie(HttpResponse response, string token);
    }
}
