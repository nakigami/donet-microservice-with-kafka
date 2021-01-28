using Microsoft.EntityFrameworkCore;
using ProductCategoryApi.Models;

namespace ProductCategoryApi.Services
{
    public class CatalogueDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Client> Clients { get; set; }
        
        public CatalogueDbContext(DbContextOptions options):base(options)
        {
        }   
    }
}