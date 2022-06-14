using Microsoft.EntityFrameworkCore;

namespace Beetee.Models.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<HRData> HrData { get; set; }

    }
}
