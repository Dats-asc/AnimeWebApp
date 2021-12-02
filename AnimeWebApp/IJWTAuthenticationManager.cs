using AnimeWebApp.Models;

namespace AnimeWebApp
{
    public interface IJWTAuthenticationManager
    {
        public string Authenticate(User model);
    }
}