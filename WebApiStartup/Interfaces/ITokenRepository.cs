using Microsoft.AspNetCore.Identity;

namespace WebApiStartup.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
