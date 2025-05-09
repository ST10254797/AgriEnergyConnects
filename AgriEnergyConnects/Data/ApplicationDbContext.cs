using AgriEnergyConnects.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnects.Data
{ 

      public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options) // Specify <ApplicationDbContext> type
            {

            }
            public DbSet<Farmer> Farmers { get; set; }
        }
    
}
