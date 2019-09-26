using System.Threading.Tasks;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Domain.Services.Communication;

namespace SampleRestAPI.API.Domain.Services
{
    public interface IUserService
    {
        Task<QueryResult<User>> ListAsync(UsersQuery query);
        Task<UserResponse> SaveAsync(User user);
        Task<UserResponse> UpdateAsync(int id, User user);
        Task<UserResponse> DeleteAsync(int id);
    }
}