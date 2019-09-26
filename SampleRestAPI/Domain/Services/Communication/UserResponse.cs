using SampleRestAPI.API.Domain.Models;

namespace SampleRestAPI.API.Domain.Services.Communication
{
    public class UserResponse : BaseResponse<User>
    {
        public UserResponse(User user) : base(user) { }

        public UserResponse(string message) : base(message) { }
    }
}