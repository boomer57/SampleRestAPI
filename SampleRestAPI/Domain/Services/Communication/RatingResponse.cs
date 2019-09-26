using SampleRestAPI.API.Domain.Models;

namespace SampleRestAPI.API.Domain.Services.Communication
{
    public class RatingResponse : BaseResponse<Rating>
    {
        public RatingResponse(Rating rating) : base(rating) { }

        public RatingResponse(string message) : base(message) { }
    }
}