using JwtWebApiDotNet7.Dto;
using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApiDotNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly PDBContext _context;


        public MessageController(PDBContext context)
        {
            _context = context;
        }


        [HttpGet("get_messages"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Messages>>> GetMessages()
        {
            return await _context.MESSAGES.ToListAsync();
        }


        [HttpGet("get_message/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Messages>> GetMessage(int id)
        {
            var message = await _context.MESSAGES.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }


        // POST: api/messages
        [HttpPost("create_message"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<MessagesDto>> PostMessage(MessagesDto request)
        {

            Messages messages = new Messages
            {
                Message = request.Message
            };
            _context.MESSAGES.Add(messages);
            await _context.SaveChangesAsync();

            return Ok(request);
        }
        [HttpGet("delete_message"), Authorize(Roles = "Admin")]
        public void Delete(int id)
        {
            var message = _context.MESSAGES.Where(x => x.Occasion_ID == id).First();
            if (message != null)
            {
                _context.MESSAGES.Remove(message);
                _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("message is not exists");

            }


        }

    }
}