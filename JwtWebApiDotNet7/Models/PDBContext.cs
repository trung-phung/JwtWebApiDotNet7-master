// using Microsoft.EntityFrameworkCore;

// namespace JwtWebApiDotNet7.Models
// {
//     public class PDBContext: DbContext {

//         String builder = @"Data Source = localhost,3306;Initial Catalog = PROJECT3_BOUQUET;User ID = root;Password =a";
//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             Console.WriteLine("Creating");
//             base.OnConfiguring(optionsBuilder);
//             optionsBuilder.UseMySQL(builder);
//         }
//     }
// }

using Microsoft.EntityFrameworkCore;

namespace JwtWebApiDotNet7.Models
{
    public class PDBContext : DbContext
    {
        public PDBContext(DbContextOptions option) : base(option)
        { }

        public DbSet<Bouquet> BOUQUET { get; set; }
        public DbSet<User> USER { get; set; }
        public DbSet<Messages> MESSAGES { get; set; }
        public DbSet<Orders> ORDERS { get; set; }
    }
}
