using System.Collections;
using JwtWebApiDotNet7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApiDotNet7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<BouquetDto>>> GetBouquets()
        {

            var list = await _context.BOUQUET.ToListAsync();
            var listDto = new ArrayList();


            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";



            foreach (var a in list)
            {
                try
                {
                    string Filepath = _environment.WebRootPath + "\\Upload\\product\\" + a.Name;
                    string imagepath = Filepath + "\\" + a.Name + ".png";
                    if (System.IO.File.Exists(imagepath))
                    {
                        Imageurl = hosturl + "/Upload/product/" + a.Name + "/" + a.Name + ".png";
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }
                BouquetDto dto = new BouquetDto();
                dto.Name = a.Name;
                dto.Price = a.Price;
                dto.Photo = Imageurl;
                dto.Id = a.Bouquet_ID;
                listDto.Add(dto);
            }
            return Ok(listDto);
        }


        [HttpGet("get_bouquet"), Authorize(Roles = "Admin,User")]
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
            BouquetDto dto = new BouquetDto()
            {
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

            return Ok(bouquet);
        }

        // DELETE: api/bouquet/5
        [HttpGet("delete"), Authorize(Roles = "Admin")]

        public void Deletebouquet(int id)
        {
            if (BouquetExists(id))
            {
                var bouquet = _context.BOUQUET.Where(x => x.Bouquet_ID == id).First();
                if (bouquet != null)
                {
                    _context.BOUQUET.Remove(bouquet);
                    _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Bouquet is not exists");

                }


            }


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