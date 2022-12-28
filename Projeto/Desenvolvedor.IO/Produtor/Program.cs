using Confluent.Kafka;
using System;

namespace Produtor
{
    class Program
    {
        static  void Main(string[] args)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<string, string>(config).Build();

            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = $"Memsagem Teste - { DateTime.Now.Second}"
            };

            var result =    producer.ProduceAsync("topico-teste", message).Result;

            Console.WriteLine($"{result.Offset}");
        }
    }
}
