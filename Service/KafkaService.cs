using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        public async Task Publish(string name, int number)
        {
            using var p = new ProducerBuilder<Null, string>(_config).Build();

            try
            {
                var dr = await p.ProduceAsync("KSQL-DEMO",
                    new Message<Null, string>
                    {
                        Value = JsonConvert.SerializeObject(new
                        {
                            id = name,
                            value = number
                        })
                    });

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
