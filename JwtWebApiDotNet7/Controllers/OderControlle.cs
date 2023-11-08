using System.Security.Claims;
using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace JwtWebApiDotNet7.Controllers
{
    [Route("oder/[controller]")]
    [ApiController]

    public class OderController : ControllerBase
    {
        private readonly PDBContext _context;


        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return _context.USER.Where(x => x.Username == claimsIdentity.Name).First();
        }
        public OderController(PDBContext context)
        {
            _context = context;
        }


        [HttpGet("admin_get_oders"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Oders>>> GetAdminOders()
        {
            return await _context.ODERS.ToListAsync();
        }



        [HttpGet("get_my_oders"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Oders>>> GetOders()
        {

            return _context.ODERS.Where(a => a.UserID == GetUser().ID).ToList();
        }


        [HttpGet("get_oder/{id}"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Oders>> GetMessage(int id)
        {
            var oder = await _context.ODERS.FindAsync(id);

            if (oder == null)
            {
                return NotFound();
            }
            var user = GetUser();
            if (user.ID != oder.UserID && user.UserRole != "Admin")
            {
                throw new Exception("Không có quyền xem đơn hàng của người khác");
            }
            return oder;
        }


        // POST: api/oders
        [HttpPost("create_oder"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Oders>> PostMessage(Oders request)
        {
            request.Status = "I";
            request.UserID = GetUser().ID;
            _context.ODERS.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Create_Oder", new { id = request.ID }, request);
        }




        // [HttpPut("update_oder"), Authorize(Roles = "Admin,User")]
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

        [HttpGet("approve_oder/{id}"), Authorize(Roles = "Admin")]
        public async void Putuser(int id)
        {

            var oder = await _context.ODERS.FindAsync(id);
            if (oder == null)
            {
                throw new Exception("oder is not exists");
            }
            oder.Status = "A";
            await _context.SaveChangesAsync();
        }


        // DELETE: api/oder/5
        [HttpGet("cance/{id}"), Authorize(Roles = "Admin,User")]

        public async void Canceluser(int id)
        {
            var oder = await _context.ODERS.FindAsync(id);
            if (oder == null)
            {
                throw new Exception("oder is not exists");
            }
            if (oder.Status == "I")
            {
                var user = GetUser();
                if (user.UserRole == "Admin" || (user.ID == oder.UserID))
                {
                    oder.Status = "C";
                    _context.ODERS.Remove(oder);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("Không thể hủy đơn");
            }
        }

    }
}