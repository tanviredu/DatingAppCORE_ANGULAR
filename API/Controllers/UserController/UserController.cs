using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.UserController
{
    public class UserController:BaseApiController{
        private readonly DataContext _context;
        public UserController(DataContext context){
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users = await _context.Users.ToListAsync();
            return users;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id){
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

    }
}