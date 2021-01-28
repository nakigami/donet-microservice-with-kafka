using ProductCategoryApi.Models;

namespace ProductCategoryApi.Services
{
    public static class DbInit
    {
        public static void InitData(CatalogueDbContext catalogueDbContext)
        {
            catalogueDbContext.Categories.Add(new Category("Electronics"));
            catalogueDbContext.Categories.Add(new Category("Technology"));
            catalogueDbContext.Products.Add(new Product("Mac book pro", 1800,1));
            catalogueDbContext.Products.Add(new Product("Imprimante Dell M4200", 900,1));
            catalogueDbContext.Products.Add(new Product("Micro Ondes", 800,2));
            catalogueDbContext.Clients.Add(new Client("Anas RIANI", "anas.devriani@gmail.com", "0612415263"));
            catalogueDbContext.Clients.Add(new Client("Yassine RIANI", "yassine.riani@gmail.com", "0641525753"));
            catalogueDbContext.SaveChanges();
        }
    }
}