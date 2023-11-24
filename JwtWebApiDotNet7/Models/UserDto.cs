namespace JwtWebApiDotNet7.Models
{
    public class UserDto
    {

        public string FName { get; set; }
        public string LName { get; set; }
        // public  DateTime DOB {get;set;}
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
