using System.Threading.Tasks;
using SampleRestAPI.API.Domain.Repositories;
using SampleRestAPI.API.Persistence.Contexts;

namespace SampleRestAPI.API.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;     
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}