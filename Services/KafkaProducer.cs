using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCategoryApi.Services
{
    public class KafkaProducer
    {
        private string _topicName;
        private IProducer<string,string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();
        private string key;
        
        public KafkaProducer(string topicName, string key)
        {
            var config = new ProducerConfig{

                BootstrapServers = "localhost:9092"
            };
            this._topicName = topicName;
            this._config = config;
            this._producer = new ProducerBuilder<string,string>(this._config).Build();
            this.key = key;
        }
        public async Task WriteMessage(string message){
            var dr = await this._producer.ProduceAsync(this._topicName, new Message<string, string>()
            {
                Key = this.key,
                Value = message
            });
            Console.WriteLine($"KAFKA => Delivered Key : {dr.Key} | Value : '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            return;
        }
    }
}