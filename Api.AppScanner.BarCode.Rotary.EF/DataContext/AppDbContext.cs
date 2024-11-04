using Api.AppScanner.BarCode.Rotary.COMMON.Models;
using Api.AppScanner.BarCode.Rotary.EF.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Api.AppScanner.BarCode.Rotary.EF.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CodigoBarrasMap());
        }

        public DbSet<CodigoBarrasModel> CodigoBarrasModel { get; set; }

    }
}
