using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;

namespace Produtor
{
    class Program
    {
        static  void Main(string[] args)
        {
            var schemaConfig =new  SchemaRegistryConfig{
                Url = "http://localhost:8081"
            };

            var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);


            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<string, desenvolvedor.io.Curso>(config)
                .SetValueSerializer(new AvroSerializer<desenvolvedor.io.Curso>(schemaRegistry))
                .Build();

            var message = new Message<string, desenvolvedor.io.Curso>
            {
                Key = Guid.NewGuid().ToString(),
                Value = new desenvolvedor.io.Curso{
                    id = Guid.NewGuid().ToString(),
                    descricao = "Curso de Apache Kafka"
                }
            };

            var result =    producer.ProduceAsync("cursos", message).Result;

            Console.WriteLine($"{result.Offset}");
        }
    }
}
