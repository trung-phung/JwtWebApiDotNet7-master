using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApiDotNet7.Controllers
{
    [ApiController]
    [Route("bouquet/[controller]")]
    public class BouquetController : ControllerBase
    {
        private readonly PDBContext _context;


        public BouquetController(PDBContext context)
        {
            _context = context;
        }


        [HttpGet("get_bouquets"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Bouquet>>> GetBouquets()
        {
            return await _context.BOUQUET.ToListAsync();
        }


        [HttpGet("get_bouquet/{id}"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Bouquet>> GetBouquet(int id)
        {
            var bouquet = await _context.BOUQUET.FindAsync(id);

            if (bouquet == null)
            {
                return NotFound();
            }

            return bouquet;
        }


        // POST: api/BOUQUET
        [HttpPost("create_bouquet"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Bouquet>> PostBouquet(BouquetDto bouquetDto)
        {
            Bouquet bouquet = new Bouquet
            {
                Name = bouquetDto.Name,
                Price = bouquetDto.Price
            };
            _context.BOUQUET.Add(bouquet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBouquet", new { id = bouquet.Bouquet_ID }, bouquet);
        }




        [HttpPut("update_bouquet"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Bouquet>> PutBouquet(Bouquet bouquet)
        {
            _context.Entry(bouquet).State = EntityState.Modified;
            if (BouquetExists(bouquet.Bouquet_ID))
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Bouquet is not exists");
            }

            return bouquet; //204 No Content
        }

        // DELETE: api/bouquet/5
        [HttpDelete("delete/{id}"), Authorize(Roles = "Admin")]
        
        public async void Deletebouquet(int id)
        {
            var bouquet = await _context.BOUQUET.FindAsync(id);
            if (bouquet == null)
            {
                throw new Exception("Bouquet is not exists");
            }

            _context.BOUQUET.Remove(bouquet);
            await _context.SaveChangesAsync();
        
        }
        private bool BouquetExists(int id)
        {
            return _context.BOUQUET.Any(e => e.Bouquet_ID == id);
        }
    }
}