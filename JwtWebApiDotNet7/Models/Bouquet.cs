using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    public class Bouquet
    {
        [Key]
        public int Bouquet_ID { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = "";

        public int Price { get; set; }
    }
}
