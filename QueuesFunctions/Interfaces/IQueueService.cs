using System.Threading.Tasks;

namespace QueuesFunctions.Interfaces
{
    public interface IQueueService
    {
        Task Delete();
        Task Insert(string message);
        Task<string> Retrieve();
    }
}
