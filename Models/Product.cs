using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCategoryApi.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        
        public string Name { get; set; }
        
        public float Price { get; set; }
        
        public long CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        
        public Product(){}

        public Product(string Name, float Price, long CategoryId)
        {
            this.Name = Name;
            this.Price = Price;
            this.CategoryId = CategoryId;
        }
    }
}