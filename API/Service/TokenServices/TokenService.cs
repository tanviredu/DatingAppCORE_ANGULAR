using API.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;

namespace API.Service.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config){
            // take the random token key in your appsettings.json
            // this key will be responsible for encrypting and
            // decrypting the token
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user){
            var claims = new List<Claim> {
                // adding properties to the jwt token
                // in this case just the name 
                // in the nameid
                // in here you add the Name in the json web token
                // adding a single property
                // we just take the username from the user
                // give user and the token will
                // save the name inside it
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
            };
            // createthe credentials with the key that you have
            // made in the constructor
            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            
            // describe how the token represents
            // what info you want
            // we take the subject which is the token
            // expires 
            // and the credentials from the key that will be
            // used to encrypt and decrypt(sining) the dat
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = cred,
            };
            // create a token andler
            var tokenhandler = new JwtSecurityTokenHandler();
            // create the token
            var token = tokenhandler.CreateToken(tokenDiscriptor);
            // write the token
            return tokenhandler.WriteToken(token);

        }
    }
}
