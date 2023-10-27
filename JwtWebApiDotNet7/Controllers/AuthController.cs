using JwtWebApiDotNet7.Dto;
using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtWebApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PDBContext _context;
        public AuthController(IConfiguration configuration,PDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }



        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            
            if(_context.USER.Any(e => e.Username == request.Username)){
                return BadRequest("User already exists");
            }
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new User();
            user.Username = request.Username;
            user.Password = passwordHash;
            user.UserRole = "User";
            _context.USER.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("User", new { id = user.ID }, user);
        }

        private async Task CreateAsync(User user){
            _context.USER.Add(user);
            await _context.SaveChangesAsync();
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(LoginDto request)
        {
            // var user =  _context.USER.SingleOrDefault(mUser => mUser.Username == request.Username);
            // var userEF =  from b in _context.USER
            //        where b.Username == request.Username
            //        select b;
            // var user = _context.USER
            //         .Where(b => b.Username == request.Username)
            //         .FirstOrDefault();

            var user = _context.USER.Single(x => x.Username == request.Username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
