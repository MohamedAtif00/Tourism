using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tourism.Model;

namespace Tourism.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> categories { get; set; } 
        public DbSet<TourismPlace> tourismPlaces { get; set; }
        public DbSet<Review> reviews { get; set; }  
        public DbSet<Activity> activities { get; set; }



    }
}
