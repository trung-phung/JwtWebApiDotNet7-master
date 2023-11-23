using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    public class BouquetDto
    {
        public string Name { get; set; } = "";

        public int Price { get; set; }
        public string? Photo;


    }
}
