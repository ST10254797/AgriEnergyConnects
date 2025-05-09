using AgriEnergyConnects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AgriEnergyConnects.Data
{ 

      public class ApplicationDbContext : IdentityDbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options) // Specify <ApplicationDbContext> type
            {

            }
            public DbSet<Farmer> Farmers { get; set; }
        }
    
}
