using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCategoryApi.Models;
using ProductCategoryApi.Services;

namespace ProductCategoryApi.Controllers
{
    [Route("/api/categories")]
    public class CategoryRestController : Controller
    {
        private CatalogueDbContext catalogueDbContext;

        public CategoryRestController(CatalogueDbContext catalogueDbContext)
        {
            this.catalogueDbContext = catalogueDbContext;
        }
               
        [HttpGet]
        public IEnumerable<Category> Index()
        {
            return this.catalogueDbContext.Categories;
        }

        [HttpGet("{Id}")]
        public Category GetOne(long Id)
        {
            return this.catalogueDbContext.Categories.FirstOrDefault(c => c.CategoryId == Id);
        }

        [HttpGet("{Id}/products")]
        public IEnumerable<Product> Products(long Id)
        {
            return this.catalogueDbContext
                .Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == Id)?
                .Products;
        }

        [HttpPost]
        public async Task<Category> Save([FromBody] Category category)
        { 
            this.catalogueDbContext.Categories.Add(category);
            this.catalogueDbContext.SaveChanges();

            string serializedCategory = JsonSerializer.Serialize(category);

            Console.WriteLine("====> CategoryRestController : Saving new category");
            Console.WriteLine(serializedCategory);

            var producer = new KafkaProducer("tp_dot_net_spring", "new-category-saving");
            await producer.WriteMessage(serializedCategory);
            
            return category;
        }

        [HttpPut("{Id}")]
        public async Task<Category> Update([FromBody] Category category, long Id)
        {
            category.CategoryId = Id;
            this.catalogueDbContext.Update(category);
            this.catalogueDbContext.SaveChanges();
            
            string serializedCategory = JsonSerializer.Serialize(category);

            Console.WriteLine("====> CategoryRestController : Updating a category");
            Console.WriteLine(serializedCategory);

            var producer = new KafkaProducer("tp_dot_net_spring", "category-updating");
            await producer.WriteMessage(serializedCategory);
            
            return category;
        }
        
        [HttpDelete("{Id}")]
        public async Task Delete(long Id)
        {
            var category = this.catalogueDbContext.Categories.FirstOrDefault(c => c.CategoryId == Id);
            this.catalogueDbContext.Categories.Remove(category);
            this.catalogueDbContext.SaveChanges();
            
            string serializedCategory = JsonSerializer.Serialize(category);

            Console.WriteLine("====> CategoryRestController : Deleting a category");
            Console.WriteLine(serializedCategory);

            var producer = new KafkaProducer("tp_dot_net_spring", "category-deleting");
            await producer.WriteMessage(serializedCategory);
        }
    }
}