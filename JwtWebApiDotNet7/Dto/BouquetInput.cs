using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    public class BouquetInput
    {
        public string Name { get; set; } = "";
        public IFormFile? Photo { get; set; } = null;
        public int Price { get; set; }



    }
}
