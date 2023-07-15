using FindMe.Models;

namespace FindMe.Services
{
    public interface IHashPassword
    {
        SaltHashedPassword GetHashedPassword(string password);
    }
}
