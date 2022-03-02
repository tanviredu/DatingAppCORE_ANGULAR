using API.Data;
using API.DTOs;
using API.Entity;
using API.Service.TokenServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers.UserController{
    public class AccountController:BaseApiController{
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context,ITokenService tokenService){
            _context = context;
            _tokenService = tokenService;

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
            if( await UserExists(registerDto.Username)){
                return BadRequest("UserName is taken");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _context.Users.SingleOrDefaultAsync(s => s.UserName == loginDto.Username);
            if (user == null) { return Unauthorized("Invaid username"); }
            var salt = user.PasswordSalt;
            using var hmac = new HMACSHA512(salt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // now check with a forloop
            for(int i = 0; i < computeHash.Length; i++){
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        private async Task<bool>UserExists(string username){
            return await _context.Users.AnyAsync(s => s.UserName == username.ToLower());
        }

        
        
    }
}