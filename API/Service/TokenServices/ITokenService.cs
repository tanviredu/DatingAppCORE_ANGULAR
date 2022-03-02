using API.Entity;

namespace API.Service.TokenServices
{
    public interface ITokenService{
        string CreateToken(AppUser user);
    }
}
