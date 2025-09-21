using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace tourismApplication.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Tour> Tours { get; set; }



    }
}
