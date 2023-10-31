using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    public class Messages
    {
        [Key]
        public int Occasion_ID { get; set; }


        public string Message { get; set; }


    }
}
