using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtWebApiDotNet7.Models
{
    
    public class User
    {
        public User(){}
        [Key]
        public int ID { get; set; }

        [Required]
        [Column("User_Role")]
        public string UserRole {get;set;}

        [Column("F_Name")]
        public  string? FName {get;set;}

        [Column("L_Name")]
        public  string? LName {get;set;}

        [Column("DOB")]
        public  DateTime? DOB {get;set;}

        [Column("Gender")]
        public  string? Gender {get;set;}

        [Column("P_No")]
        public  string? Phone {get;set;}
    
        [Column("Address")]
        public  string? Address {get;set;}

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
