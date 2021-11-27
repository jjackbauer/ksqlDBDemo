using System.Threading.Tasks;

namespace ksqlDBDemo.Service
{
    public interface IKafkaService
    {
        Task Publish(string name, int value);
    }
}
