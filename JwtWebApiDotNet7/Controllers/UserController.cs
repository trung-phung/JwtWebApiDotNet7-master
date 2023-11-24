using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PDBContext _context;


        public UserController(PDBContext context)
        {
            _context = context;
        }


        [HttpGet("get_users"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> Getusers()
        {
            return await _context.USER.ToListAsync();
        }


        [HttpGet("get_user"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Getuser(int id)
        {
            var user = await _context.USER.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // POST: api/user
        [HttpPost("create_user"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Postuser(UserDto request)
        {
            string passwordHash
    = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new User
            {
                Username = request.Username,
                Password = passwordHash,
                UserRole = "Admin",
                Gender = request.Gender,
                FName = request.FName,
                LName = request.LName,
                Address = request.Address,
                Phone = request.Phone
            };
            _context.USER.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }




        // [HttpPut("update_user"), Authorize(Roles = "Admin")]
        // public async Task<ActionResult<User>> Putuser(User user)
        // {
        //     _context.Entry(user).State = EntityState.Modified;
        //     if (userExists(user.ID))
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     else
        //     {
        //         throw new Exception("user is not exists");
        //     }

        //     return user; //204 No Content
        // }

        // DELETE: api/user/5
        [HttpGet("delete"), Authorize(Roles = "Admin")]

        public void Deleteuser(int id)
        {
            var user = _context.USER.Where(x => x.ID == id).First();
            if (user == null)
            {
                throw new Exception("user is not exists");
            }

            _context.USER.Remove(user);
            _context.SaveChangesAsync();

        }
        private bool userExists(int id)
        {
            return _context.USER.Any(e => e.ID == id);
        }
    }
}