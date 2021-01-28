using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductCategoryApi.Models;
using ProductCategoryApi.Services;

namespace ProductCategoryApi.Controllers
{
    [Route("api/clients")]
    public class ClientRestController : Controller
    {
        private CatalogueDbContext _catalogueDbContext;

        public ClientRestController(CatalogueDbContext catalogueDbContext)
        {
            _catalogueDbContext = catalogueDbContext;
        }
        
        public IEnumerable<Client> Index()
        {
            return _catalogueDbContext.Clients;
        }

        [HttpGet("{Id}")]
        public Client getOne(long id)
        {
            Client client = _catalogueDbContext.Clients.FirstOrDefault(c => c.ClientId == id);
            return client;
        }
        
        [HttpPost]
        public async Task<Client> Save([FromBody] Client client)
        {
            _catalogueDbContext.Clients.Add(client);
            _catalogueDbContext.SaveChanges();
            
            
            string serializedClient = JsonSerializer.Serialize(client);

            Console.WriteLine("====> ClientRestController : Saving new client");
            Console.WriteLine(serializedClient);

            var producer = new KafkaProducer("tp_dot_net_spring", "new-client-saving");
            await producer.WriteMessage(serializedClient);

            return client;
        }

        [HttpDelete("{Id}")]
        public async Task<string> DeleteClient(long id)
        {
            Client client = _catalogueDbContext.Clients.FirstOrDefault(c => c.ClientId == id);
            _catalogueDbContext.Clients.Remove(client);
            _catalogueDbContext.SaveChanges();
            
            string serializedClient = JsonSerializer.Serialize(client);

            Console.WriteLine("====> ClientRestController : Deleting a client");
            Console.WriteLine(serializedClient);

            var producer = new KafkaProducer("tp_dot_net_spring", "client-deleting");
            await producer.WriteMessage(serializedClient);

            return "Deleted Successfully !";
        }

        [HttpPut("{Id}")]
        public async Task<Client> Update(long id, [FromBody] Client client)
        {
            client.ClientId = id;
            _catalogueDbContext.Clients.Update(client);
            _catalogueDbContext.SaveChanges();
            
            string serializedClient = JsonSerializer.Serialize(client);

            Console.WriteLine("====> ClientRestController : Updating a client");
            Console.WriteLine(serializedClient);

            var producer = new KafkaProducer("tp_dot_net_spring", "client-updating");
            await producer.WriteMessage(serializedClient);
            
            return client;
        }
    }
}