using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApiDotNet7.Controllers
{
    [Route("message/[controller]")]
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
        public async Task<ActionResult<Messages>> PostMessage(Messages request)
        {

            Messages messages = new Messages
            {
                Message = request.Message
            };
            _context.MESSAGES.Add(messages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Create_Mess", new { id = messages.Occasion_ID }, messages);
        }

    }
    }