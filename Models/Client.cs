using System.ComponentModel.DataAnnotations;

namespace ProductCategoryApi.Models
{
    public class Client
    {
        public long ClientId { get; set; }
        
        [Required, StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Phone { get; set; }

        public Client()
        {
            
        }

        public Client(string name, string email, string phone)
        {
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
        }
    }
}