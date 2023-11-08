using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    public class Oders
    {
        [Key]
        public int ID { get; set; }


        public string? ReceiverName { get; set; }

        public string? DeliveryAddress { get; set; }

        public int? Phone {get; set; }

        public DateTime? Date{get; set;}

        public int? UserID { get; set; }

        public int? OccasionID{ get; set; }

        public string?  CustomMessage { get; set; }

        public string?  Status { get; set; }

    }
}
