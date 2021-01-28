using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductCategoryApi.Models

{
    public class Category
    {

        [Required]
        public long CategoryId { get; set; }
        
        [Required, StringLength(20)]
        public string CategoryName { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
        
        public Category(){}

        public Category(string CategoryName)
        {
            this.CategoryName = CategoryName;
        }
    }
}