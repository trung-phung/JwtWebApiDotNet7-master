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
        private readonly IWebHostEnvironment _environment;

        public BouquetController(PDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet("get_bouquets"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Bouquet>>> GetBouquets()
        {
            return await _context.BOUQUET.ToListAsync();
        }


        [HttpGet("get_bouquet/{id}"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<BouquetDto>> GetBouquet(int id)
        {
            var bouquet = await _context.BOUQUET.FindAsync(id);

            if (bouquet == null)
            {
                return NotFound();
            }

            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = _environment.WebRootPath + "\\Upload\\product\\" + bouquet.Name;
                string imagepath = Filepath + "\\" + bouquet.Name + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    Imageurl = hosturl + "/Upload/product/" + bouquet.Name + "/" + bouquet.Name + ".png";
                }
                else
                {
                }
            }
            catch (Exception)
            {
            }
            // string photoURL = GetImage(bouquet.Name);
            BouquetDto dto = new BouquetDto(){
                Name = bouquet.Name,
                Price = bouquet.Price,
                Photo = Imageurl
            };
            return dto;
        }



        // POST: api/BOUQUET
        [HttpPost("create_bouquet"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Bouquet>> PostBouquet(string name, int price, IFormFile formFile)
        {

            Bouquet bouquet = new Bouquet
            {
                Name = name,
                Price = price
            };
            UploadImage(formFile, name);
            _context.BOUQUET.Add(bouquet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBouquet", new { id = bouquet.Bouquet_ID }, bouquet);
        }


        // public string? GetImage(string bouquetName)
        // {
        //     string Imageurl = string.Empty;
        //     string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        //     try
        //     {
        //         string Filepath = _environment.WebRootPath + "\\Upload\\product\\" + bouquetName;
        //         string imagepath = Filepath + "\\" + bouquetName + ".png";
        //         if (System.IO.File.Exists(imagepath))
        //         {
        //             Imageurl = hosturl + "/Upload/product/" + bouquetName + "/" + bouquetName + ".png";
        //         }
        //         else
        //         {
        //             return null;
        //         }
        //     }
        //     catch (Exception)
        //     {
        //     }
        //     return Imageurl;

        // }


        // [HttpPut("update_bouquet"), Authorize(Roles = "Admin")]
        // public async Task<ActionResult<Bouquet>> PutBouquet(string name, int price, IFormFile formFile)
        // {
        //     _context.Entry(bouquet).State = EntityState.Modified;
        //     if (BouquetExists(bouquet.Bouquet_ID))
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     else
        //     {
        //         throw new Exception("Bouquet is not exists");
        //     }
        //     UploadImage(formFile, name);
        //     return bouquet; //204 No Content
        // }

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


        private async void UploadImage(IFormFile formFile, string bouquetName)
        {
            try
            {
                string Filepath = _environment.WebRootPath + "\\Upload\\product\\" + bouquetName;
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\" + bouquetName + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);

                }
            }
            catch (Exception)
            {
                throw new Exception("Error save photo");
            }
        }

    }
}