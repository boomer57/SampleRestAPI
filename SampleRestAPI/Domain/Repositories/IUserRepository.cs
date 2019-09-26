using System.Collections.Generic;
using System.Threading.Tasks;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;

namespace SampleRestAPI.API.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<QueryResult<User>> ListAsync(UsersQuery query);
        Task AddAsync(User user);
        Task<User> FindByIdAsync(int id);
        void Update(User user);
        void Remove(User user);
    }
}