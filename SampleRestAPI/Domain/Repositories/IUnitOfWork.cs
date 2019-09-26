using System.Threading.Tasks;

namespace SampleRestAPI.API.Domain.Repositories
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}