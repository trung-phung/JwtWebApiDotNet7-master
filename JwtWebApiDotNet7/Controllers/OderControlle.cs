using System.Security.Claims;
using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace JwtWebApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly PDBContext _context;


        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return _context.USER.Where(x => x.Username == claimsIdentity.Name).First();
        }
        public OrderController(PDBContext context)
        {
            _context = context;
        }


        [HttpGet("admin_get_orders"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetAdminOrders()
        {
            return await _context.ORDERS.ToListAsync();
        }



        [HttpGet("get_my_orders"), Authorize(Roles = "Admin,User")]
        public ActionResult<IEnumerable<Orders>> GetOrders()
        {

            return _context.ORDERS.Where(a => a.User_ID == GetUser().ID).ToList();
        }


        [HttpGet("get_order"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Orders>> GetMessage(int id)
        {
            var order = await _context.ORDERS.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            var user = GetUser();
            if (user.ID != order.User_ID && user.UserRole != "Admin")
            {
                throw new Exception("Không có quyền xem đơn hàng của người khác");
            }
            return Ok(order);
        }


        // POST: api/orders
        [HttpPost("create_order"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Orders>> PostMessage(Orders request)
        {
            request.Status = "I";
            request.User_ID = GetUser().ID;
            _context.ORDERS.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }




        // [HttpPut("update_order"), Authorize(Roles = "Admin,User")]
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

        [HttpGet("approve_order"), Authorize(Roles = "Admin")]
        public void Approve(int id)
        {

            var order = _context.ORDERS.Where(x => x.ID == id).First();
            if (order == null)
            {
                throw new Exception("order is not exists");
            }
            order.Status = "A";
            _context.SaveChangesAsync();
        }


        // DELETE: api/order/5
        [HttpGet("cancel"), Authorize(Roles = "Admin,User")]

        public void Cancel(int id)
        {

            var order = _context.ORDERS.Where(x => x.ID == id).First();
            if (order == null)
            {
                throw new Exception("order is not exists");
            }
            if (order.Status == "I")
            {
                var user = GetUser();
                if (user.UserRole == "Admin" || (user.ID == order.User_ID))
                {
                    order.Status = "C";
                    _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("Không thể hủy đơn");
            }
        }

    }
}