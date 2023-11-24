using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    public class Orders
    {
        [Key]
        public int ID { get; set; }


        public string? Receiver_Name { get; set; }

        public string? Delivery_Address { get; set; }

        public int? Phone { get; set; }

        public DateTime? Date { get; set; }

        public int? User_ID { get; set; }

        public int? Bouquet_ID { get; set; }

        public int? Occasion_ID { get; set; }

        public string? Custom_Message { get; set; }

        public string? Status { get; set; }

    }
}
