using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategoryApi.Models;
using ProductCategoryApi.Services;

namespace ProductCategoryApi.Controllers
{
    [Route("/api/products")]
    public class ProductRestController : Controller
    {
        private CatalogueDbContext catalogueDbContext;

        public ProductRestController(CatalogueDbContext catalogueDbContext)
        {
            this.catalogueDbContext = catalogueDbContext;
        }
        
        [HttpGet]
        public IEnumerable<Product> Index()
        {
            return this.catalogueDbContext.Products.Include(p => p.Category);
        }

        [HttpGet("search")]
        public IEnumerable<Product> FindProducts(string kw)
        {
            return this.catalogueDbContext
                .Products
                .Include(p=> p.Category)
                .Where(p => p.Name.Contains(kw));
        }
        
        [HttpGet("{Id}")]
        public Product GetOne(long id)
        {
            return this.catalogueDbContext
                .Products
                .Include(p=> p.Category)
                .FirstOrDefault(p => p.ProductId == id);
        }

        [HttpGet("paginate")]
        public IEnumerable<Product> Paginate(int page=0, int size=1)
        {
            int skipValue = (page - 1) * size;

            return this.catalogueDbContext
                .Products
                .Include(p => p.Category)
                .Skip(skipValue)
                .Take(size);
        }
        
        [HttpPost]
        public async Task<Product> Save([FromBody] Product product)
        {
            this.catalogueDbContext.Products.Add(product);
            this.catalogueDbContext.SaveChanges();

            string serializedCategory = JsonSerializer.Serialize(product);

            Console.WriteLine("====> ProductRestController : Saving new product");
            Console.WriteLine(serializedCategory);

            var producer = new KafkaProducer("tp_dot_net_spring", "saving-new-product");
            await producer.WriteMessage(serializedCategory);
            
            return product;
        }

        [HttpPut("{Id}")]
        public async Task<Product> Update([FromBody] Product product, long id)
        {
            product.ProductId = id;
            this.catalogueDbContext.Products.Update(product);
            this.catalogueDbContext.SaveChanges();
            
            string serializedCategory = JsonSerializer.Serialize(product);

            Console.WriteLine("====> ProductRestController : Updating a product");
            Console.WriteLine(serializedCategory);

            var producer = new KafkaProducer("tp_dot_net_spring", "updating-a-product");
            await producer.WriteMessage(serializedCategory);
            return product;
        }

        [HttpDelete("{Id}")]
        public async Task Delete(long id)
        {
            var product = this.catalogueDbContext.Products.FirstOrDefault(p => p.ProductId == id);
            this.catalogueDbContext.Products.Remove(product);
            this.catalogueDbContext.SaveChanges();
            
            string serializedCategory = JsonSerializer.Serialize(product);

            Console.WriteLine("====> ProductRestController : Deleting a product");
            Console.WriteLine(serializedCategory);

            var producer = new KafkaProducer("tp_dot_net_spring", "deleting-a-product");
            await producer.WriteMessage(serializedCategory);
        }
    }
}