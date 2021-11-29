using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ksqlDBDemo.Service
{
    public class KafkaService : IKafkaService
    {
        private readonly ProducerConfig _config;
        public KafkaService(IConfiguration configuration )
        {
            _config = new ProducerConfig
            {
                BootstrapServers = configuration.GetValue<string>("KafkaBootstrap"),
                ClientId = Dns.GetHostName()
            };
        }
        public async Task Publish(string topic, string message)
        {
            using var p = new ProducerBuilder<Null, string>(_config).Build();

            try
            {
                var dr = await p.ProduceAsync(topic,
                    new Message<Null, string>
                    {
                        Value = message
                    }) ;

                Console.WriteLine($"Kafka producer -- {DateTime.Now.ToString()} -- {dr.Topic} -- {dr.Value}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Kafka producer -- {DateTime.Now.ToString()} -- ERROR -- {e.Error.Reason} ");
            }
            finally
            {
                p.Dispose();
            }

            return;
        }
    }
}
