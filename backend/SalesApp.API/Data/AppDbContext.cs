using Microsoft.EntityFrameworkCore;
using SalesApp.API.Data.Entities;

namespace SalesApp.API.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleDetail> SaleDetail { get; set; }
    }
}