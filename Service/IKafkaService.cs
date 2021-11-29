using System.Threading.Tasks;

namespace ksqlDBDemo.Service
{
    public interface IKafkaService
    {
        Task Publish(string topic, string message);
    }
}
