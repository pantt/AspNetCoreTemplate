using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreFrame.Entities.Models;

namespace NetCoreFrame.Repository.EF
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public IConfiguration Configuration { get; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connStr = Configuration.GetConnectionString("DefaultConnection");
        //    optionsBuilder.UseMySql(connStr);
        //}

        public DbSet<User> User { get; set; }
        public DbSet<Sample> Sample { get; set; }
    }
}